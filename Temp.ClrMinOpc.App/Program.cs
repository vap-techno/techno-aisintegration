using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Graybox.OPC.ServerToolkit.CLRWrapper;


namespace Temp.ClrMinOpc.App
{
    class Program
    {

        // This int will be set to 1 when it's time to exit.
        static int stop = 0;
        // Count of OPC tags to create.
        static int tag_count = 2;
        // The TagId identifiers of OPC tags.
        static int[] tag_ids = new int[tag_count];

        static OPCDAServer srv = new OPCDAServer();

        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {
            // This will be the CLSID and the AppID of our COM-object.
            Guid srv_guid = new Guid("3464aeca-ba04-4343-9a01-944d8e1aa873");
            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and return.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srv_guid,
                            "Technologia",
                            "TehcnoAisOpc",
                            "TAO",
                            "0.1");
                        return;
                    }
                    // Remove the OPC server registration and return.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(srv_guid);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            // Create an object of the OPC Server class.
            //OPCDAServer srv = new OPCDAServer();

            // Advise for the OPC Toolkit events.
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srv_guid, 50, 50, ServerOptions.NoAccessPaths, '.', 100);
            
            // Create the OPC tags.
            // Create a tag.
            tag_ids[0] = srv.CreateTag(0, "Node.AIS.Task.Cmd", AccessRights.readWritable, "");
            tag_ids[1] = srv.CreateTag(1, "Node.AIS.Task.Resposne", AccessRights.readWritable, "");

            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();
            // Wait until the OPC server is released by the clients.
            // Periodically update tags values while the OPC server is not released.
            while (System.Threading.Interlocked.CompareExchange(ref stop, 1, 1) == 0)
            {

                System.Threading.Thread.Sleep(100);
                // Begin the update of the OPC server cache.
                srv.BeginUpdate();

                // Finish the update of the OPC server cache. We pass false,
                // because its unnecessary for this update to be synchronous.
                srv.EndUpdate(false);
            }
            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();

        }

        /// <summary>
        /// A handler for the WriteItems event of the OPCDAServer object.
        /// We do not update the OPC server cache here.
        /// </summary>
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {

            Console.WriteLine(sender);

            for (int i = 0; i < e.Count; i++)
            {
                if (e.ItemIds[i].TagId == 0) continue;
                try
                {
                    // Cmd
                    if (e.ItemIds[i].TagId == 1)
                    {

                        // TODO: Вставить кусок логики, а пока просто передаем вход на выход
                        Thread.Sleep(3000);
                        
                        string val = e.Values[i].ToString();
                        if (val != String.Empty || val != "")
                        {
                            
                            MemoryExchange mmc = new MemoryExchange();
                            var res = mmc.WriteCmd(val);

                            Console.WriteLine(res);

                            // Записываем в тег Response
                            var resp = mmc.ReadResponse();
                            Console.WriteLine(resp);

                            srv.SetTag(2, resp, Quality.Good, FileTime.UtcNow);



                            // Стираем тэг
                            srv.SetTag(1, "", Quality.Good, FileTime.UtcNow);

                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Errors[i] = (ErrorCodes)System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                    e.ItemIds[i].TagId = 0;
                    e.MasterError = ErrorCodes.False;
                }
            }
        }

        /// <summary>
        /// A handler for the ServerReleased event of the OPCDAServer object.
        /// </summary>
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Make the OPC server object 'suspended'.
            // No new OPC server instances can be created by the clients
            // from this moment.
            e.Suspend = true;
            // Signal the main thread, that it's time to exit.
            System.Threading.Interlocked.Exchange(ref stop, 1);
        }





    }
}
