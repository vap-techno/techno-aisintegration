using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Response;
using AisJson.Lib.DTO.Task;
using AisJson.Lib.Utils;
using AisOpcClient.Lib;
using AutoMapper;
using DAL.Core.TaskMapper;
using DAL.Entity;
using DAL.Entity.Abstract;
using DAL.Entity.Status;
using DAL.InMemory;
using DAL.UnitOfWork;
using Newtonsoft.Json;
using Serilog;

namespace BL.Core
{
    public class Manager 
    {
        //TODO: Во всех методах убрать создание экземпляра репозитория, вынести их в Fields
        // TODO: При необходимости создать несколько менеджеров для каждой команды, объединив их под интерфейсом

        #region Fields

        private const int sleep = 10;
    
        private readonly TaskMapper _taskMapper;
        private readonly ILogger _logger;

        // Коды статусов поста налива
        private const int FS_NONE = 0;
        private const int FS_STANDBY = 1;
        private const int FS_INPROGRESS = 2;
        private const int FS_CANCELED = 3;
        private const int FS_ERROR = 4;
        private const int FS_NOTFOUND = 5;
        private const int FS_COMPLETED = 6;

        private JsonSerializerSettings formatSettings; // Настройки отображения времени 

        #endregion

        #region Properties

        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public string DbConString { get; }

        #endregion

        #region Constructors

        public Manager(string dbConString, TaskMapper mapper, ILogger logger)
        {
            DbConString = dbConString;
            _logger = logger;
            _taskMapper = mapper;

            // Инициализация
            try
            {
                Init();
            }
            catch (Exception e)
            {
                _logger.Error(e,"Не инициализируется менедежер.");
                throw;
            }

            formatSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
            };

        }

        #endregion ctor

        private void Init()
        {
            // TODO: Вставить логику или удалить
        }

        /// <summary>
        /// Обработать JSON-команду с заявками
        /// </summary>
        /// <param name="json">JSON-команда</param>
        /// <returns></returns>
        public string HandleRequest(string json)
        {
            // Преобразуем JSON-строку в список заявок формата DTO
            List<IRequestDto> cmdDtoList = AisJConverter.Deserialize(json, _logger);

            if (cmdDtoList != null && cmdDtoList.Count > 0)
            {
                // Очищаем пустые команды
                cmdDtoList.RemoveAll(item => item == null);

                // Обрабатываем все команды
                List<IResponseDto> respDtoList = new List<IResponseDto>();

                // Валидация команд
                var isTasksValid = cmdDtoList.All(dto =>
                {
                    if (!dto.Validate())
                    {
                        _logger.Warning("Команда {Cmd} CID = {Cid} не прошла валидацию", dto.Cmd, dto.Cid);
                        Thread.Sleep(sleep);
                    }

                    return dto.Validate();
                });

                // Если одна из завявок не прошла валидацию, то возвращаем ошибку
                if (!isTasksValid)
                    return JsonConvert.SerializeObject(new
                        {Error = "Команда не прошла валидацию", ErrCode = -2147024809, Ts = DateTime.Now}, formatSettings);


                // После полной валидации обрабатываем заявки        
                cmdDtoList.ForEach(dto =>
                {
                    var hList = HandleTask(dto);
                    hList?.ForEach(item => respDtoList.Add(item));
                });

                // Очищаем пустые команды и пишем в OPC-сервер ответы
                respDtoList.RemoveAll(item => item == null);

                // Если все команды не требуют ответа, то возвращаем просто диагностическую информацию
                if (respDtoList.Count == 0) return JsonConvert.SerializeObject(new { Status = "Запрос обработан", Ts = DateTime.Now }, formatSettings);
                return JsonConvert.SerializeObject(respDtoList, formatSettings);
            }

            return JsonConvert.SerializeObject(new { Error = "Не удалось обработать запрос", ErrCode = -1073479676, Ts = DateTime.Now }, formatSettings);
        }

        /// <summary>
        /// Получить стаутс задания исходя из статусов секций
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public int GetScField(FillInTask task)
        {
            try
            {
                // Если хотя бы один "В обработке", то все возвращаем "В обработке"
                bool inProgress = task.Details.Any((item) => item.Fs == FS_INPROGRESS);
                if (inProgress) return FS_INPROGRESS;

                // Равные статусы по всем секциям = статус задания
                for (int i = FS_NONE; i <= FS_COMPLETED; i++)
                {
                    bool res = task.Details.All((item) => item.Fs == i);
                    if (res) return i;
                }

                // Если какая-то секция отменена, ошибочна или выполнена, а какая-то еще принята к исполнению - то заявка все еще в работе
                bool stillProgress = task.Details.Any((item) => item.Fs == FS_STANDBY) && task.Details.Any((item) => item.Fs >= FS_CANCELED);
                if (stillProgress) return FS_INPROGRESS;

                // Если все секции завершились, то статус заявки выдаем по наименьшей
                bool completed = task.Details.Any((item) => item.Fs >= FS_CANCELED);
                if (completed)
                {
                    var fsList = new List<int>();
                    task.Details.ForEach((item) => fsList.Add(item.Fs));
                    return fsList.Min();
                }
            }
            catch (Exception e)
            {
                _logger.Warning(e, "Не получилось получить статус заявки.");
                Thread.Sleep(sleep);
                return FS_NOTFOUND;
            }

            // Если что-то не предусмотрел, то возвращаем "Не найдено, или не может быть предоставлен"
            return FS_NOTFOUND; 
        }

        /// <summary>
        /// Получить стаутс задания исходя из статусов секций
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public int GetScField(FillOutTask task)
        {

            try
            {
                // Если хотя бы один "В обработке", то все возвращаем "В обработке"
                bool inProgress = task.Details.Any((item) => item.Fs == FS_INPROGRESS);
                if (inProgress) return FS_INPROGRESS;

                // Равные статусы по всем секциям = статус задания
                for (int i = FS_NONE; i <= FS_COMPLETED; i++)
                {
                    bool res = task.Details.All((item) => item.Fs == i);
                    if (res) return i;
                }

                // Если какая-то секция отменена, ошибочна или выполнена, а какая-то еще принята к исполнению - то заявка все еще в работе
                bool stillProgress = task.Details.Any((item) => item.Fs == FS_STANDBY) && task.Details.Any((item) => item.Fs >= FS_CANCELED);
                if (stillProgress) return FS_INPROGRESS;

                // Если все секции завершились, то статус заявки выдаем по наименьшей
                bool completed = task.Details.Any((item) => item.Fs >= FS_CANCELED);
                if (completed)
                {
                    var fsList = new List<int>();
                    task.Details.ForEach((item) => fsList.Add(item.Fs));
                    return fsList.Min();
                }
            }
            catch (Exception e)
            {
                _logger.Warning(e,"Не получилось получить статус заявки.");
                Thread.Sleep(sleep);
                return FS_NOTFOUND;
            }

            // Если что-то не предусмотрел, то возвращаем "Не найдено, или не может быть предоставлен"
            return FS_NOTFOUND;
        }

        #region Task Handlers

        /// <summary>
        /// Обработка заявки, пришедшей из АИС ТПС
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<IResponseDto> HandleTask(IRequestDto dto)
        {

            if (dto is FillInTaskDto) // Обработка запроса на налив в АЦ
            {
                return HandleFillInTask(dto); 
            }
            else if (dto is FillInMcTaskDto) // Обработка запроса на налив КМХ
            {
                return HandleFillInMcTask(dto); 
            }
            else if (dto is FillOutTaskDto) // Обработка запроса на слив из АЦ
            {
                return HandleFillOutTask(dto); 
            }
            else if (dto is StatusTaskDto)
            {
                return HandleStatusTask(dto);
            }
            else if (dto is CancelTaskDto) // Обработка запроса на отмену 
            {
                return HandleCancelTask(dto);
            }


            return null;
        }

        /// <summary>
        /// Обработка заявки Налив в АЦ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<IResponseDto> HandleFillInTask(IRequestDto dto)
        {
            // Преобразуем DTO в Entity
            var task = Mapper.Map<FillInTask>(dto); ;

            // Ищем по идентификатору в базе данных
            var rep = new FIllInTaskRepository(DbConString, _logger);
            var foundedTask = rep.GetItem(task.AisTaskId);

            // Если запись существует - пишем в лог сообщение
            if (foundedTask != null)
            {
                _logger.Warning("Заявка НАЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} уже существует в базе данных ", task.AisTaskId, task.Cid);
                Thread.Sleep(sleep);
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            task.Details.ForEach(d =>
            {
                d.Fs = FS_STANDBY;
                d.Ls = FS_STANDBY;
            });
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка НАЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);
            Thread.Sleep(sleep);
            return null;
        }

        /// <summary>
        /// Обработка заявки Налив в АЦ КМХ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<IResponseDto> HandleFillInMcTask(IRequestDto dto)
        {
            // Преобразуем DTO в Entity
            var task = Mapper.Map<FillInMcTask>(dto);

            // Ищем по идентификатору в базе данных
            var rep = new FIllInMcTaskRepository(DbConString, _logger);
            var foundedTask = rep.GetItem(task.AisTaskId);

            // Если запись существует - пишем в лог сообщение
            if (foundedTask != null)
            {
                _logger.Warning("Заявка НАЛИВ КМХ. Заявка ID = {AisTaskId} CID = {Cid} уже существует в базе данных ", task.AisTaskId, task.Cid);
                Thread.Sleep(sleep);
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            task.Fs = 1;
            task.Ls = 1;
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка НАЛИВ КМХ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);
            Thread.Sleep(sleep);

            return null;
        }

        /// <summary>
        /// Обработка заявки Налив в АЦ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<IResponseDto> HandleFillOutTask(IRequestDto dto)
        {
            // Преобразуем DTO в Entity
            var task = Mapper.Map<FillOutTask>(dto);

            // Ищем по идентификатору в базе данных
            var rep = new FIllOutTaskRepository(DbConString, _logger);
            var foundedTask = rep.GetItem(task.AisTaskId);

            // Если запись существует - пишем в лог сообщение
            if (foundedTask != null)
            {
                _logger.Warning("Заявка СЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} уже существует в базе данных ", task.AisTaskId, task.Cid);
                Thread.Sleep(sleep);
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            task.Details.ForEach(d =>
            {
                d.Fs = FS_STANDBY;
                d.Ls = FS_STANDBY;
            });
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка СЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);
            Thread.Sleep(sleep);

            return null;
        }

        /// <summary>
        /// Обработка заявки Отмена
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<IResponseDto> HandleCancelTask(IRequestDto dto)
        {
            // Преобразуем DTO в Entity
            // CancelTaskDto ==> CancelTask
            var task = Mapper.Map<CancelTask>(dto);

            // Записываем в БД
            var rep = new CancelTaskRepository(DbConString, _logger);

            var res = rep.Create(task);
            if (res > 0)
            {
                // Ищем заявку по всем таблицам

                var cancelResponseLst = new List<CancelResponse>();

                // Ищем и отменяем заявки по всем таблицам
                foreach (var aisId in task.Ids)
                {
                    CancelResponse resp = null;

                    // Вначале пытаемся отменить в таблице FillInTask (Налив в АЦ)
                    resp = CancelTaskInFillIn(aisId, dto.Cid);
                    if (resp == null)
                    {
                        // Пытаемся отменить в таблице FillInMcTask (Налив в АЦ КМХ)
                        resp = CancelTaskInFillInMc(aisId, dto.Cid);

                        if (resp == null)
                        {
                            // Пытаемся отменить в таблице FillOutTask (Слив из АЦ)
                            resp = CancelTaskInFillOut(aisId, dto.Cid);

                            if (resp == null)
                            {
                                resp = new CancelResponse()
                                {
                                    Id = aisId,
                                    Cid = dto.Cid,
                                    R = false,
                                    Rm = "Заявка не найдена",
                                    Ts = new StatusResponse()
                                    {
                                        Id = aisId,
                                        Cid = dto.Cid,
                                        Sc = FS_NOTFOUND,
                                        Rm = "Заявка не найдена",
                                    }
                                };

                                // Если заявки не обнаружено ни в одной из таблице - формируем сообщение в лог
                                _logger.Warning("Заявка на ОТМЕНУ. Заявка на отмену с ID = {aisId} не найдена", aisId);
                                Thread.Sleep(sleep);
                            }
                        }
                    }

                    cancelResponseLst.Add(resp);
                }

                // Преобразуем CancelResponse ==> CancelResponseDto
                var cancelResponseListDto = new List<IResponseDto>();

                cancelResponseLst.ForEach(item => cancelResponseListDto
                    .Add(_taskMapper.GetCancelResponseDto(item)));

                return cancelResponseListDto;

            }

            _logger.Warning("Заявка на ОТМЕНУ. Заявка с CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", task.Cid);
            Thread.Sleep(sleep);
            // TODO: Вернуть класс с флагом ошибки
            return null;
        }

        /// <summary>
        /// Обработка заявки Статус
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<IResponseDto> HandleStatusTask(IRequestDto dto)
        {
            // Преобразуем DTO в Entity
            // StatusTaskDto ===> StatusTask
            var task = Mapper.Map<StatusTask>(dto);

            // Записываем в БД
            var rep = new StatusTaskRepository(DbConString, _logger);

            var res = rep.Create(task);
            if (res > 0)
            {
                // Ищем заявку по всем таблицам
                var statusResponseLst = new List<StatusResponse>();

                // Ищем и отменяем заявки по всем таблицам
                foreach (var aisId in task.Ids)
                {
                    StatusResponse resp = null;

                    // Вначале пытаемся в таблице FillInTask (Налив в АЦ)
                    resp = StatusTaskFillIn(aisId, dto.Cid);
                    if (resp == null)
                    {
                        // Пытаемся в таблице FillInMcTask (Налив в АЦ КМХ)
                        resp = StatusTaskFillInMc(aisId, dto.Cid);

                        if (resp == null)
                        {
                            // Пытаемся в таблице FillOutTask (Слив из АЦ)
                            resp = StatusTaskFillOut(aisId, dto.Cid);

                            if (resp == null)
                            {
                                // Если заявки не обнаружено ни в одной из таблице - формируем сообщение в лог
                                resp = new StatusResponse()
                                {
                                    Id = aisId,
                                    Cid = dto.Cid,
                                    Sc = FS_NOTFOUND,
                                    Rm = "Заявки не существует"
                                };

                                _logger.Warning("Заявка СТАТУС. Заявка на статус с ID = {aisId} не найдена", aisId);
                                Thread.Sleep(sleep);
                            }
                        }
                    }

                    statusResponseLst.Add(resp);
                }

                // Преобразуем CancelResponse ==> CancelResponseDto
                var statusResponseListDto = new List<IResponseDto>();

                statusResponseLst.ForEach(item => statusResponseListDto
                    .Add(_taskMapper.GetStatusResponseDto(item)));

                return statusResponseListDto;

            }

            _logger.Warning("Заявка СТАТУС. Заявка на статус с CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", task.Cid);
            Thread.Sleep(sleep);

            return null;
        }

        #endregion

        #region Status Task Handlers

        private StatusResponse StatusTaskFillIn(string aisId, string statusCid)
        {
            FIllInTaskRepository rep = new FIllInTaskRepository(DbConString, _logger);
            StatusResponse response = null;

            // Ищем заявку в таблице FillInTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask == null || foundedTask.FillInTaskId <= 0) return null;
            
            // Формируем Entity ответа
            response = new StatusResponse()
            {
                Id = foundedTask.AisTaskId,
                Cid = statusCid,
                Sc = GetScField(foundedTask),
                Rm = "Статус из таблицы налива в АЦ",
                Sd = Mapper.Map<FillInStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы НАЛИВ АЦ", foundedTask.Cid);
            Thread.Sleep(sleep);

            return response;
        }

        private StatusResponse StatusTaskFillInMc(string aisId, string statusCid)
        {
            FIllInMcTaskRepository rep = new FIllInMcTaskRepository(DbConString, _logger);
            StatusResponse response = null;

            // Ищем заявку в таблице FillInMcTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask == null || foundedTask.FillInMcTaskId <= 0) return null;

            // Формируем Entity ответа
            response = new StatusResponse()
            {
                Id = foundedTask.AisTaskId,
                Cid = statusCid,
                Sc = foundedTask.Fs,
                Rm = "Статус из таблицы Налив КМХ",
                Sd = Mapper.Map<FillInMcStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы НАЛИВ КМХ", foundedTask.Cid);
            Thread.Sleep(sleep);

            return response;
        }

        private StatusResponse StatusTaskFillOut(string aisId, string statusCid)
        {
            FIllOutTaskRepository rep = new FIllOutTaskRepository(DbConString, _logger);
            StatusResponse response = null;

            // Ищем заявку в таблице FillOutTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask == null || foundedTask.FillOutTaskId <= 0) return null;

            // Формируем Entity ответа
            response = new StatusResponse()
            {
                Id = foundedTask.AisTaskId,
                Cid = statusCid,
                Sc = GetScField(foundedTask),
                Rm = "Статус из таблицы слива из АЦ",
                Sd = Mapper.Map<FillOutStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы СЛИВ АЦ", foundedTask.Cid);
            Thread.Sleep(sleep);

            return response;
        }


        #endregion

        #region CancelTaskHandlers

        /// <summary>
        /// Попытка отменить заявку в таблице FillInTask
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <param name="cancelCid"></param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillIn(string aisId, string cancelCid)
        {
            FIllInTaskRepository rep = new FIllInTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillInTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask != null && foundedTask.FillInTaskId > 0)
            {
                // Заявка отменяется только в секциях со статусом "К исполнению"

                // Список sid секций, которые были отменены
                List<string> canceledList = new List<string>();

                // Ищем не начатые заявки, устанавливаем статус "Отменено"
                foundedTask.Details.ForEach(item =>
                {
                    if (item.Fs == FS_STANDBY)
                    {
                        item.Fs = FS_CANCELED;
                        item.Ls = FS_CANCELED;
                        canceledList.Add(item.Sid);
                    }
                });

                // Обновляем заявку в базе данных
                if (rep.Update(foundedTask))
                {
                    // Формируем Entity ответа

                    var strSidList = (canceledList.Count > 0 ? String.Join(",", canceledList) : "");

                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = cancelCid,
                        R = true,
                        Rm = $"Отменено {canceledList.Count} : {strSidList}",
                        Ts = new StatusResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            Sc = GetScField(foundedTask),
                            Rm = "Статус из таблицы налива в АЦ",
                            Sd = Mapper.Map<FillInStatusDetail>(foundedTask)
                        }
                    };

                    _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из НАЛИВ АЦ отменены: {strSidList}",
                        foundedTask.Cid, strSidList);
                    Thread.Sleep(sleep);
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = cancelCid,
                        R = false,
                        Rm = "Ошибка взаимодействия с базой данных АСН",
                        Ts = new StatusResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            Sc = FS_ERROR,
                            Rm = "Ошибка"
                        }
                    };

                    _logger.Warning(
                        "Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН",
                        foundedTask.Cid);
                    Thread.Sleep(sleep);
                }
            }

            return response;
        }

        /// <summary>
        /// Попытка отменить заявку в таблице FillInMcTask
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <param name="cancelCid"></param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillInMc(string aisId, string cancelCid)
        {
            FIllInMcTaskRepository rep = new FIllInMcTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillInMcTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask != null && foundedTask.FillInMcTaskId > 0)
            {
                // Заявку можно отменить, если она находится в статусе "К исполнению"
                if (foundedTask.Fs == FS_STANDBY)
                {
                    // Помечаем статус в "Отменено"
                    foundedTask.Fs = FS_CANCELED;
                    if (rep.Update(foundedTask))
                    {
                        // Формируем Entity ответа
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            R = true,
                            Rm = "Отменено",
                            Ts = new StatusResponse()
                            {
                                Id = foundedTask.AisTaskId,
                                Cid = cancelCid,
                                Sc = foundedTask.Fs,
                                Rm = "Статус из таблицы Налив КМХ",
                                Sd = Mapper.Map<FillInMcStatusDetail>(foundedTask)
                            }
                        };

                        _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из НАЛИВ КМХ отменена", foundedTask.Cid);
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        // Формируем Entity с ошибкой отмены
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            R = false,
                            Rm = "Ошибка взаимодействия с базой данных АСН",
                            Ts = new StatusResponse()
                            {
                                Id = foundedTask.AisTaskId,
                                Cid = cancelCid,
                                Sc = FS_ERROR,
                                Rm = "Ошибка"
                            }

                        };
                        _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", foundedTask.Cid);
                        Thread.Sleep(sleep);
                    }
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = cancelCid,
                        R = true,
                        Rm = "Заявка уже в работе, либо завершена",
                        Ts = new StatusResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            Sc = foundedTask.Fs,
                            Rm = "Статус из таблицы Налив КМХ",
                            Sd = Mapper.Map<FillInMcStatusDetail>(foundedTask)
                        }
                    };
                    _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} из НАЛИВ КМХ, отказ - заявка в работе или завершена", foundedTask.Cid);
                    Thread.Sleep(sleep);
                }
            }

            return response;

        }

        /// <summary>
        /// Попытка отменить заявку в таблице FillOut
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <param name="cancelCid"></param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillOut(string aisId, string cancelCid)
        {
            FIllOutTaskRepository rep = new FIllOutTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask != null && foundedTask.FillOutTaskId > 0)
            {
                // Заявка отменяется только в секциях со статусом "К исполнению"

                // Список sid секций, которые были отменены
                List<string> canceledList = new List<string>();

                // Ищем не начатые заявки, устанавливаем статус "Отменено"
                foundedTask.Details.ForEach(item =>
                {
                    if (item.Fs == FS_STANDBY)
                    {
                        item.Fs = FS_CANCELED;
                        item.Ls = FS_CANCELED;
                        canceledList.Add(item.Sid);
                    }
                });

                // Обновляем заявку в базе данных
                if (rep.Update(foundedTask))
                {
                    // Формируем Entity ответа

                    var strSidList = (canceledList.Count > 0 ? String.Join(",", canceledList) : "");

                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = cancelCid,
                        R = true,
                        Rm = $"Отменено {canceledList.Count} : {strSidList}",
                        Ts = new StatusResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            Sc = GetScField(foundedTask),
                            Rm = "Статус из таблицы слива из АЦ",
                            Sd = Mapper.Map<FillInStatusDetail>(foundedTask)
                        }
                    };

                    _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из СЛИВ АЦ отменены: {strSidList}",
                        foundedTask.Cid, strSidList);
                    Thread.Sleep(sleep);
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = cancelCid,
                        R = false,
                        Rm = "Ошибка взаимодействия с базой данных АСН",
                        Ts = new StatusResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = cancelCid,
                            Sc = FS_ERROR,
                            Rm = "Ошибка"
                        }
                    };

                    _logger.Warning(
                        "Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН",
                        foundedTask.Cid);
                    Thread.Sleep(sleep);
                }

            }

            return response;
        }

        #endregion


    }
}