# Change Tracking Tool
Change Tracking Tool - former CRM Comparer

For the original CRM Comparer tool sources, check out this link: https://archive.codeplex.com/?p=crmcomparer

## Change Tracking Tool Release 1

### Downloads

Dynamics solution file: https://github.com/ashlega/CRMComparer/blob/master/sourceCode/Solution/ChangeTracking_1_0_0_0.zip
Executable: https://github.com/ashlega/CRMComparer/blob/master/sourceCode/Builds/ChangeTrackingTool_1.0.0.0.zip

After downloading ChangeTrackingTool_1.0.0.0.zip, make sure to unblock the zip file (from the file properties)

### Setup

Note: this build has been tested with 8.2 version of Dynamics

- Import solution file to Dynamics
- Extract the contents of ChangeTrackingTool to a separate folder

If you wanted to compare solution files, you can run the executable, and, for the two files there, you can pick either solution zip files or customization xml files.

If you wanted to start tracking updates directly in Dynamics, you'll have to follow these steps:

- Open ChangeTrackingTool.exe.config in notepad
- Update Dynamics connection string to match your instance of Dynamics. For the sample connection string, please have a look here: https://msdn.microsoft.com/en-us/library/mt608573.aspx
- Start the tool using the following command line(you can use windows command prompt, batch file, powershell, etc): ChangeTrackingTool.exe SolutionName

The first time you start it, there will be no change log in Dynamics. For all subsequent starts, the tool will be comparing solution content with the results of the previous start, so, if there are any changes, you will start seeing change log records in Dynamics. To see the records, open Advanced Find and choose Change Log entity there


