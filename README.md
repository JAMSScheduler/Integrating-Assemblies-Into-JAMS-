# Integrating-Assemblies-Into-JAMS-
Sample project demonstrating how integrating outside .dll assemblies into a JAMS execution Method. This enables JAMS the ability to fire off methods from an outside library, from a JAMS execution method. Currently we are only demonstrating this ability using hard coded values that are being passed into the JAMS Job Source Tab comboBox. We do not yet have a working example of doing this dynamically but will consider for future examples. 

This repo consists of two projects: 

1. CustomProject
2. JAMSSourceAndHost

-CustomProject represents a project outside of JAMS that is already built and contains working methods, parameters, variables...etc. This project generates the .dll file that JAMS will expose allowing users to integrate their .dll files into JAMS jobs. 

-JAMSSourceAndHost is a class that has been built, that will essentially tie the CustomProject and JAMS together. This project has three classes: 

1. Host.cs
2. Source.cs
3. SourceView.cs

-Host.cs This class must reference the CustomProject library; it also must implement IJAMSHost. IJAMSHost requires the following methods: Initialize, Execute, Cleanup and Cancel. In this class we set one parameter requirement for the job called "WriteCount". This is optional, but it demonstrates how parameters for the job are set. This is done in the Initialize method. The Execute method is where the majority of the logic is handled. Basically in this method we read in our parameter "WriteCount" and it's value. Read in the source from the Job "Source" Tab. We then determine which CustomProject method was selected from the comboBox and fire off the method from their library. In this example, I also hard coded the parameters being passed into the two CustomProject methods. This can also be set through the JAMS client as well. Then we create a log explaining which method we are firing off and we execute.  

-Source.cs is a class that will handle the data from the "Source" tab. It uses the JavaScriptSerializer to get the source and then import and deserialize it. This is where properties are created that will be used in Host.cs so that the CustomProject dll can be exposed. 

-SourceView.cs is the class that builds the GUI for the Job "Source" tab. This class will create all of the properties that will be used in the "Source" tab as well as demonstrate where and how we are adding the CustomProject methods into the comboBox used in this example. This class must implement IEditJobSource   


The last step to pull the libraries and also newly created class (JAMSSourceAndHost) into JAMS is to copy the assemblies and move them into JAMS. When copying the assemblies you will want the following. 

- CustomProject.dll
- CustomProject.pdb
- JAMSSourceAndHost.dll
- JAMSSourceAndHost.pdb

## Utilization
You will paste those in the both directories(You will need to close JAMS and make sure the JAMSScheduler service is stopped): 
-MVPSI/JAMS/Client
-MVPSI/JAMS/Scheduler
*Note*: If you do not create a view for Job Source you only need to paste in the Scheduler

Then you will need to create a job with the "SqlCommand" execution method. Go to the "Execution" tab in the Execution Method and make sure the following is set: 

-Type: Should be set to "Routine"
-Routine Location:
  -Assembly: This will be the name of your assembly mine is JAMSSourceAndHost
  -Class: This will be the class where IJAMSHost is implemented mine is JAMSSourceAndHost.Host
-Source Editor: 
  -Assembly: This will be the name of your assembly min is JAMSSourceAndHost
  -Class: This will be the class where you're source controls are built mine is JAMSSourceAndHost.SourceView
  
You should now be able to attach a Job to this this Execution Method and run this job normally with the exception that you should now have a custom Source Tab with the integrated assembly methods selectable. 
  
 
