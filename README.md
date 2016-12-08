# Integrating-Assemblies-Into-JAMS-
Sample project demonstrating how integrating outside .dll assemblies into a JAMS execution Method.  This enables JAMS the ability to fire off methods from an outside library, from a JAMS execution method.  Currently we are only demonstrating this ability using hard coded values that are being passed into the JAMS Job Source Tab comboBox.  We do not yet have a working example of doing this dynamically but will consider for future examples.  

The zip file consists of two project: 

1. CustomProject
2. JAMSSourceAndHost

-CustomProject represents a project outside of JAMS that is already built and contains working methods, parameters, variables...etc. This project generates the .dll file that JAMS will expose allowing users to integrate their .dll files into JAMS jobs. 

-JAMSSourceAndHost is a class that has been built that will essentially tie the CustomProject and JAMS together.  This project has three classes: 

1. Host.cs
2. Source.cs
3. SourceView.cs

-Host.cs This class must reference the CustomProject library. It also must implement IJAMSHost.  IJAMSHost requires the following methods: Initialize, Execute, Cleanup and Cancel.  In this class we set one parameter requirement for the job called "WriteCount".  This is optional, but it demonstrates how parameters for the job are set.  This is done in the Initialize method.  The Execute method is where the majority of the logic is handled.  Basically in this method we read in our parameter "WriteCount" and it's value.  Read in the source from the Job "Source" Tab. We then determine which CustomProject method was selected from the comboBox and fire off the method from their library.  In this example, I also hard coded the parameters being passed into the two CustomProject methods.  This can also be set through the JAMS client as well.  Then we create a log explaining which method we are firing off and we execute.    

-Source.cs is a class that will handle the data from the "Source" tab.  It uses the JavaScriptSerializer to get the source and then import and deserialize it.  This is where properties are created that will be used in Host.cs so that the CustomProject dll can be exposed. 

-SourceView.cs is the class that builds the GUI for the Job "Source" tab.  This class will create all of the properties that will be used in the "Source" tab as well as demonstrate where and how we are adding the CustomProject methods into the comboBox used in this example.  This class must implement IEditJobSource      
