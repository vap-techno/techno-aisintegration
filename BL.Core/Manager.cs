using System;
using System.CodeDom;
using System.Collections.Generic;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Response;
using AisJson.Lib.DTO.Task;
using AisJson.Lib.Utils;
using AisOpcClient.Lib;
using AutoMapper;
using DAL.Core.TaskMapper;
using DAL.Entity;
using DAL.Entity.Status;
using DAL.UnitOfWork;
using Newtonsoft.Json;
using Serilog;

namespace BL.Core
{
    public class Manager 
    {
        //TODO: Во всех методах убрать создание экземпляра репозитория, вынести их в Fields
        
        #region Fields

        private OpcService _opcService;
        private TaskMapper _taskMapper;
        private readonly ILogger _logger;

        #endregion

        #region Properties
        /// <summary>
        /// URL OPC-сервера
        /// </summary>
        public Uri OpcServerUri { get; }

        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public string DbConString { get; }



        #endregion

        #region Constructors

        public Manager(string dbConString, TaskMapper mapper, OpcService opcService,  ILogger logger)
        {
            DbConString = dbConString;
            _logger = logger;
            _opcService = opcService;
            _taskMapper = mapper;

            // Инициализация
            try
            {
                Init();
            }
            catch (Exception e)
            {
                _logger.Error("Не инициализируется менедежер. {e}", e);
                throw;
            }
            
        }

        #endregion ctor

        private void Init()
        {

            // Подписываемся на события
            _opcService.ValueChanged += OpcService_ValueChanged;
            _opcService.MonitoringCanceled += OpcService_MonitoringCanceled;

            // Запускаем мониторинг
            _opcService.RunCmdMonitoring();
            
        }

        private void OpcService_MonitoringCanceled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpcService_ValueChanged(object sender, TagEventArgs<string> e)
        {
    
            // Преобразуем JSON-строку в список заявок формата DTO
            List<IRequestDto> cmdDtoList = AisJConverter.Deserialize(e.Tag.Value, _logger);

            if (cmdDtoList != null)
            {
                // Очищаем пустые команды
                cmdDtoList.RemoveAll(item => item == null);

                // Обрабатываем все команды
                List<IResponseDto> respDtoList = new List<IResponseDto>();
                cmdDtoList.ForEach(dto =>
                {
                    if (!dto.Validate())
                    {
                        _logger.Warning("Команда {Cmd} CID = {Cid} не прошла валидацию", dto.Cmd, dto.Cid);
                    }
                    else
                    {
                        var hList = HandleTask(dto);
                        hList?.ForEach(item => respDtoList.Add(item));
                    }
                });

                // Очищаем пустые команды и пишем в OPC-сервер ответы
                respDtoList.RemoveAll(item => item == null);
                _opcService.WriteResponse(JsonConvert.SerializeObject(respDtoList));
            }

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
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            // TODO: Убрать магическое число
            task.Details.ForEach(d => d.Fs = 1);
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка НАЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);
            

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
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            task.Fs = 1;
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка НАЛИВ КМХ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);

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
                return null;
            }

            // Еслии записи в БД не найдено - добавляем запись со статусом к "Исполнению"
            // TODO: Убрать магическое число
            task.Details.ForEach(d => d.Fs = 1);
            var id = rep.Create(task);

            // Записываем в лог результат
            _logger.Information("Заявка СЛИВ АЦ. Заявка ID = {AisTaskId} CID = {Cid} добавлена к исполнению, ID записи = {id}", task.AisTaskId, task.Cid, id);

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
                    resp = CancelTaskInFillIn(aisId);
                    if (resp == null)
                    {
                        // Пытаемся отменить в таблице FillInMcTask (Налив в АЦ КМХ)
                        resp = CancelTaskInFillInMc(aisId);

                        if (resp == null)
                        {
                            // Пытаемся отменить в таблице FillOutTask (Слив из АЦ)
                            resp = CancelTaskInFillOut(aisId);

                            if (resp == null)
                            {
                                resp = new CancelResponse()
                                {
                                    Id = aisId,
                                    Cid = "",
                                    R = true,
                                    Rm = "Заявка не найдена"
                                };
                                
                                // Если заявки не обнаружено ни в одной из таблице - формируем сообщение в лог
                                _logger.Warning("Заявка на ОТМЕНУ. Заявка на отмену с ID = {aisId} не найдена", aisId);
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

                    // Вначале пытаемся отменить в таблице FillInTask (Налив в АЦ)
                    resp = StatusTaskFillIn(aisId);
                    if (resp == null)
                    {
                        // Пытаемся отменить в таблице FillInMcTask (Налив в АЦ КМХ)
                        resp = StatusTaskFillInMc(aisId);

                        if (resp == null)
                        {
                            // Пытаемся отменить в таблице FillOutTask (Слив из АЦ)
                            resp = StatusTaskFillOut(aisId);

                            if (resp == null)
                            {
                                // Если заявки не обнаружено ни в одной из таблице - формируем сообщение в логd
                                resp = new StatusResponse()
                                {
                                    Id = aisId,
                                    Cid = "",
                                    Sc = 5,
                                    Rm = "Заявки не существует"
                                };

                                _logger.Warning("Заявка СТАТУС. Заявка на отмену с ID = {aisId} не найдена", aisId);
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

            _logger.Warning("Заявка СТАТУС. Заявка на отмену с CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", task.Cid);

            return null;
        }

        #endregion

        #region Status Task Handlers

        private StatusResponse StatusTaskFillIn(string aisId)
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
                Cid = foundedTask.Cid,
                Sc = 0, // TODO: Добавить вычисление статуса заявки по статусам из секций
                Rm = "Статус из таблицы налива в АЦ",
                Ts = Mapper.Map<FillInStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы НАЛИВ АЦ", foundedTask.Cid);

            return response;
        }

        private StatusResponse StatusTaskFillInMc(string aisId)
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
                Cid = foundedTask.Cid,
                Sc = foundedTask.Fs,
                Rm = "Статус из таблицы Налив КМХ",
                Ts = Mapper.Map<FillInMcStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы НАЛИВ КМХ", foundedTask.Cid);

            return response;
        }

        private StatusResponse StatusTaskFillOut(string aisId)
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
                Cid = foundedTask.Cid,
                Sc = 0, // TODO: Добавить вычисление статуса заявки по статусам из секций
                Rm = "Статус из таблицы слива из АЦ",
                Ts = Mapper.Map<FillOutStatusDetail>(foundedTask)
            };

            _logger.Information("Команда на СТАТУС. Заявка CID = {Cid} из таблицы СЛИВ АЦ", foundedTask.Cid);

            return response;
        }


        #endregion

        #region CancelTaskHandlers

        /// <summary>
        /// Попытка отменить заявку в таблице FillInTask
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillIn(string aisId)
        {
            FIllInTaskRepository rep = new FIllInTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillInTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask !=null && foundedTask.FillInTaskId > 0)
            {
                // Заявку можно отменить, если все секции заявки в статусе "К исполнению"
                // TODO: Убрать магические числа
                var foundedTaskDetail = foundedTask.Details.Find(item => item.Fs > 1);
                if (foundedTaskDetail == null)
                {
                    // Помечаем все секции в "Отменено"
                    // TODO: Убрать магические числа
                    foundedTask.Details.ForEach(item => item.Fs = 3);

                    if (rep.Update(foundedTask))
                    {
                        // Формируем Entity ответа
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = false,
                            Rm = "Отменено",
                            Ts = Mapper.Map<FillInStatusDetail>(foundedTask)
                        };

                        _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из НАЛИВ АЦ отменена", foundedTask.Cid);
                    }
                    else
                    {
                        // Формируем Entity с ошибкой отмены
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = true,
                            Rm = "Ошибка взаимодействия с базой данных АСН",
                            Ts = Mapper.Map<FillInStatusDetail>(foundedTask)
                        };

                        _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", foundedTask.Cid);
                        
                    }
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = foundedTask.Cid,
                        R = true,
                        Rm = "Заявка уже в работе, либо завершена",
                        Ts = Mapper.Map<FillInStatusDetail>(foundedTask)
                    };
                    _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} из НАЛИВ АЦ, отказ - заявка в работе или завершена", foundedTask.Cid);
                }
            }
            
            return response;
        }

        /// <summary>
        /// Попытка отменить заявку в таблице FillInMcTask
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillInMc(string aisId)
        {
            FIllInMcTaskRepository rep = new FIllInMcTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillInMcTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask != null && foundedTask.FillInMcTaskId > 0)
            {
                // Заявку можно отменить, если она находится в статусе "К исполнению"
                // TODO: Убрать магические числа
                if (foundedTask.Fs == 1)
                {
                    // Помечаем статус в "Отменено"
                    // Todo: Убрать магическое число
                    foundedTask.Fs = 3;
                    if (rep.Update(foundedTask))
                    {
                        // Формируем Entity ответа
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = false,
                            Rm = "Отменено",
                            Ts = Mapper.Map<FillInMcStatusDetail>(foundedTask)
                        };

                        _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из НАЛИВ КМХ отменена", foundedTask.Cid);
                    }
                    else
                    {
                        // Формируем Entity с ошибкой отмены
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = true,
                            Rm = "Ошибка взаимодействия с базой данных АСН",
                            Ts = Mapper.Map<FillInMcStatusDetail>(foundedTask)
                        };
                        _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", foundedTask.Cid);
                    }
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = foundedTask.Cid,
                        R = true,
                        Rm = "Заявка уже в работе, либо завершена",
                        Ts = Mapper.Map<FillInMcStatusDetail>(foundedTask)
                    };
                    _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} из НАЛИВ КМХ, отказ - заявка в работе или завершена", foundedTask.Cid);
                }
            }

            return response;

        }

        /// <summary>
        /// Попытка отменить заявку в таблице FillOut
        /// </summary>
        /// <param name="aisId">Идентификатор заявки АИС ТПС</param>
        /// <returns></returns>
        private CancelResponse CancelTaskInFillOut(string aisId)
        {
            FIllOutTaskRepository rep = new FIllOutTaskRepository(DbConString, _logger);
            CancelResponse response = null;

            // Ищем заявку в таблице FillTask
            var foundedTask = rep.GetItem(aisId);

            if (foundedTask != null && foundedTask.FillOutTaskId > 0)
            {
                // Заявку можно отменить, если все секции заявки в статусе "К исполнению"
                // TODO: Убрать магические числа
                var foundedTaskDetail = foundedTask.Details.Find(item => item.Fs > 1);
                if (foundedTaskDetail == null)
                {
                    // Помечаем все секции в "Отменено"
                    // TODO: Убрать магические числа
                    foundedTask.Details.ForEach(item => item.Fs = 3);

                    if (rep.Update(foundedTask))
                    {
                        // Формируем Entity ответа
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = false,
                            Rm = "Отменено",
                            Ts = Mapper.Map<FillOutStatusDetail>(foundedTask)
                        };

                        _logger.Information("Команда ОТМЕНА. Заявка CID = {Cid} из СЛИВ АЦ отменена", foundedTask.Cid);
                    }
                    else
                    {
                        // Формируем Entity с ошибкой отмены
                        response = new CancelResponse()
                        {
                            Id = foundedTask.AisTaskId,
                            Cid = foundedTask.Cid,
                            R = true,
                            Rm = "Ошибка взаимодействия с базой данных АСН",
                            Ts = Mapper.Map<FillOutStatusDetail>(foundedTask)
                        };
                        _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} отказ - ошибка взаимодействия с базой данных АСН", foundedTask.Cid);
                    }
                }
                else
                {
                    // Формируем Entity с ошибкой отмены
                    response = new CancelResponse()
                    {
                        Id = foundedTask.AisTaskId,
                        Cid = foundedTask.Cid,
                        R = true,
                        Rm = "Заявка уже в работе, либо завершена",
                        Ts = Mapper.Map<FillOutStatusDetail>(foundedTask)
                    };
                    _logger.Warning("Заявка на ОТМЕНУ. Заявка CID = {Cid} из СЛИВ АЦ, отказ - заявка в работе или завершена", foundedTask.Cid);
                }
            }

            return response;
        }

        #endregion

        ~Manager()
        {
            _opcService.ValueChanged -= OpcService_ValueChanged;
            _opcService.MonitoringCanceled -= OpcService_MonitoringCanceled;
            _opcService.CancelCmdMonitoring();
        }

    }
}