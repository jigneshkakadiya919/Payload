using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using TestMVC.App_Start;

namespace TestMVC
{
    /// <summary>
    /// Summary description for PostReceiveHook
    /// </summary>
    public class PostReceiveHook : IHttpHandler
    {

        //public Logger Log
        //{
        //    get;
        //    set;
        //}
        public void ProcessRequest(HttpContext context)
        {
            //Log = new Logger(context.Server.MapPath("~/log.txt"));
            //var req = context.Request;
            //Log.Log("reqest received");
            ////if (req.HttpMethod.ToLower() == "post" && !string.IsNullOrWhiteSpace(req.Form["payload"]) && req.QueryString["token"] == ConfigurationManager.AppSettings["token"])  {
            ////    Deploy();
            ////}

            //if (req.QueryString["token"] == ConfigurationManager.AppSettings["token"])
            //{
            //    Deploy();
            //}
            Deploy();
            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");
           // Log.Dispose();
        }

        private void Deploy()
        {
            ExecuteCommandSync(string.Format(@"cd {0} && git reset --hard HEAD && git pull", ConfigurationManager.AppSettings["WebRoot"]));
        }

        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        /// 
         public void ExecuteCommandSync(object command)
         {
             System.Diagnostics.ProcessStartInfo psi =
  new System.Diagnostics.ProcessStartInfo(@"C:\Users\Jignesh\Desktop\Jeff\test2.bat");
             psi.RedirectStandardOutput = true;
             psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
             psi.UseShellExecute = false;
             System.Diagnostics.Process listFiles;
             listFiles = System.Diagnostics.Process.Start(psi);
             System.IO.StreamReader myOutput = listFiles.StandardOutput;
             listFiles.WaitForExit(2000);
             //if (listFiles.HasExited == true)
             //{
                 string output = myOutput.ReadToEnd();
                 string re = output;
           //  }
             listFiles.Close();
         }
       

        //public void ExecuteCommandSync(object command)
        //{
        //    try
        //    {

        //        string strFilePath = "C:\\Users\\Jignesh\\Desktop\\Jeff\\test2.bat";

        //        // Create the ProcessInfo object
        //        System.Diagnostics.ProcessStartInfo psi =
        //                new System.Diagnostics.ProcessStartInfo("cmd.exe");
        //        psi.UseShellExecute = false;
        //        psi.RedirectStandardOutput = true;
        //        psi.RedirectStandardInput = true;
        //        psi.RedirectStandardError = true;

        //        // Start the process
        //        System.Diagnostics.Process proc =
        //                   System.Diagnostics.Process.Start(psi);

        //        // Open the batch file for reading
        //        System.IO.StreamReader strm1 =
        //                   System.IO.File.OpenText(strFilePath);
        //        System.IO.StreamReader strm = proc.StandardError;
        //        // Attach the output for reading
        //        System.IO.StreamReader sOut = proc.StandardOutput;

        //        // Attach the in for writing
        //        System.IO.StreamWriter sIn = proc.StandardInput;

        //        // Write each line of the batch file to standard input
        //        /*while(strm.Peek() != -1)
        //        {
        //            sIn.WriteLine(strm.ReadLine());
        //        }*/
        //        sIn.WriteLine("");
        //        strm.Close();

        //        // Exit CMD.EXE
        //        string stEchoFmt = "# {0} run successfully. Exiting";

        //        sIn.WriteLine(String.Format(stEchoFmt, strFilePath));
        //        sIn.WriteLine("EXIT");
        //        proc.WaitForExit();
        //        // Close the process
        //        proc.Close();

        //        // Read the sOut to a string.
        //        string results = sOut.ReadToEnd().Trim();

        //        // Close the io Streams;
        //        sIn.Close();
        //        sOut.Close();

        //        // Write out the results.
        //        //string fmtStdOut = "<font face=courier size=0>{0}</font>";
        //        //this.Response.Write("<br>");
        //        //this.Response.Write("<br>");
        //        //this.Response.Write("<br>");
        //        //this.Response.Write(String.Format(fmtStdOut,
        //        //   results.Replace(System.Environment.NewLine, "<br>")));

        //       // //Log.Log(command.ToString());
        //       // //Log.Log("begin deploy");
        //       // // create the ProcessStartInfo using "cmd" as the program to be run,
        //       // // and "/c " as the parameters.
        //       // // Incidentally, /c tells cmd that we want it to execute the command that follows,
        //       // // and then exit.                
        //       // var start = new System.Diagnostics.ProcessStartInfo("cmd.exe ", "" + command);

        //       // // The following commands are needed to redirect the standard output.
        //       // // This means that it will be redirected to the Process.StandardOutput StreamReader.
        //       // start.RedirectStandardOutput = true;
        //       // start.UseShellExecute = false;
        //       // // Do not create the black window.
        //       // start.CreateNoWindow = true;
        //       // // Now we create a process, assign its ProcessStartInfo and start it
        //       // System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //       // proc.StartInfo = start;
        //       // proc.Start();
        //       // var output = new List<string>();

        //       // while (proc.StandardOutput.Peek() > -1)
        //       // {
        //       //     output.Add(proc.StandardOutput.ReadLine());
        //       // }

        //       // while (proc.StandardError.Peek() > -1)
        //       // {
        //       //     output.Add(proc.StandardError.ReadLine());
        //       // }
        //       // proc.WaitForExit();
        //       //// proc.WaitForExit();
        //       // // Get the output into a string
        //       // string result= "";
        //       // //Log.Log(result); // Display the command output.
        //       // //Log.Log("end deploy");

        //    }
        //    catch (Exception exp)
        //    {
        //        // Log the exception
        //       // Log.LogError(exp.Message);
        //    }
        //}
        /// <summary>
        /// Execute the command Asynchronously.
        /// </summary>
        /// <param name="command">string command.</param>
        public void ExecuteCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                var thread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                thread.IsBackground = true;
                //Set the Priority of the thread.
                thread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                thread.Start(command);

               // thread.Abort();
            }
            catch (ThreadStartException exp)
            {
                // Log the exception
            }
            catch (ThreadAbortException exp)
            {
                // Log the exception
            }
            catch (Exception exp)
            {
                // Log the exception
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}