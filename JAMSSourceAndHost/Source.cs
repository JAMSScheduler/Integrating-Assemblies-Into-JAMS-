using System;
using System.Web.Script.Serialization;

namespace JAMSSourceAndHost
{
    /// <summary>
    /// WriteFileSource defines the properties that the execution method requires. These
    ///  properties are edited through the WriteFileControl on the Job's Source tab.
    /// </summary>
    public class Source
    {
        public Source() { }

        #region Properties
        /// <summary>
        /// The name of the CustomProject Method
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }
        #endregion

        #region Methods
        ///<summary>
        /// Returns the JSON Job Source
        /// </summary>
        /// <returns></returns>
        public string GetSource()
        {
            string serializedObject = String.Empty;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                //
                //  Serialize the Job source
                //
                serializedObject = serializer.Serialize(this);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while exporting WriteFileSource", ex);
            }
            return serializedObject;
        }

        ///<summary>
        ///Returns a Source object from the JSON Job Source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Source Create(string source)
        {
            Source jobSource;
            source = source.Trim();
            try
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    //
                    // Empty string, create and empty Source
                    //
                    jobSource = new Source();
                }
                else
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    // 
                    // Deserialize the Job Source from JSON
                    //
                    jobSource = serializer.Deserialize<Source>(source);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while importing Source", ex);
            }
            return jobSource;
        }
        #endregion
    }
}
