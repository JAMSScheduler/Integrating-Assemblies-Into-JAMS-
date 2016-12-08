using System;
using System.Collections.Generic;
using MVPSI.JAMS.Host;
using System.IO;
using CustomProject;

namespace JAMSSourceAndHost
{
    public class Host : IJAMSHost
    {
        private FinalResults results;
        private Source m_source;
        #region IJAMSHost Members
        /// <summary>
        /// Prepare for execution
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// /// <param name="attributes"></param>
        /// <param name="parameters">Collection of Parameters for the Job.</param>
        public void Initialize(IServiceProvider serviceProvider, Dictionary<string, object> attributes,
        Dictionary<string, object> parameters)
        {
            results = new FinalResults();
            try
            {
                //
                // Validate that the Job contains a parameter named WriteCount to indicate how many times to write the source to the file
                //
                if (!parameters.ContainsKey("WriteCount"))
                {
                    WriteErrorInfo("A Parameter named \"WriteCount\" must be specified with the number of times to write the Message to the File!", "Initialize");
                }
            }
            catch (Exception ex)
            {
                WriteErrorInfo(ex.Message, "Initialize");
            }
        }

        /// <summary>
        /// Select dll method from Custom Project
        /// </summary>
        /// <param name = "serviceProvider" ></ param >
        /// < param name="attributes"></param>
        /// <param name = "parameters" > Collection of Parameters for the Job.</param>
        /// <returns></returns>
        public FinalResults Execute(IServiceProvider serviceProvider, Dictionary<string, object> attributes,
        Dictionary<string, object> parameters)
        {
            string jobSourceFile = String.Empty;
            int writeCount = 0;
            // 
            // If there were no errors during Initialize write content to the file
            //
            if (results.FinalSeverity != 3)
            {
                try
                {
                    //
                    // Read the WriteCount parameter on the Job
                    //
                    string writeCountRaw = parameters["WriteCount"].ToString();
                    if (!String.IsNullOrWhiteSpace(writeCountRaw) && !Int32.TryParse(writeCountRaw, out writeCount))
                    {
                        WriteErrorInfo(String.Format("The value of WriteCount could not be converted to an integer: {0}", writeCountRaw), "Execute");
                    }
                    if (results.FinalSeverity != 3)
                    {
                        //
                        // Get the Job source file
                        //
                        jobSourceFile = attributes["CommandFilename"] as string;
                        //
                        // Get the deserialized Job source
                        //
                        m_source = Source.Create(File.ReadAllText(jobSourceFile));

                        //Once our source is saved and passed back to us we will determine which item was selected.  
                        //This will ultimately determine which one of our custom dll methods are being executed. 
                        if (m_source.MethodName == "CreateFileA")
                        {
                            DllMethods.CreateFileA("FileA", "C:\\");
                        }
                        else if (m_source.MethodName == "CreateFileB")
                        {
                            DllMethods.CreateFileB("FileB", "C:\\");
                        }

                        //
                        // Write to the Job Log
                        //
                        Console.WriteLine("Executing with method {0}", m_source.MethodName);
                        Console.WriteLine("Write Count: {0}", writeCount);
                        //
                        // Set the Final Results object to Success
                        //
                        results.FinalSeverity = 0;
                        results.FinalStatus = "The operation completed successfully";
                        results.FinalStatusCode = 0;
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorInfo(ex.Message, "Execute");
                }
            }
            return results;
        }

        /// <summary>
        /// Cleans up resources from execution
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="attributes"></param>
        /// <param name="parameters">Collection of Parameters for the Job.</param>
        public void Cleanup(IServiceProvider serviceProvider, Dictionary<string, object> attributes,
        Dictionary<string, object> parameters)
        {
            //
            // Cleanup any resources
            //
        }
        /// <summary>
        /// Handles the Job being canceled
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Cancel(IServiceProvider serviceProvider)
        {
            //
            // Handle the Job being cancelled
            //
        }
        #endregion
        /// <summary>
        /// Write the error message to the Job log and set the Final Results to error
        /// </summary>
        /// <param name="errorMessag"></param>
        /// <param name="methodName"></param>
        private void WriteErrorInfo(string errorMessag, string methodName)
        {
            Console.WriteLine(errorMessag);
            results.FinalSeverity = 3;
            results.FinalStatus = "Exception occured during " + methodName;
            results.FinalStatusCode = 1;
        }
    }
}
