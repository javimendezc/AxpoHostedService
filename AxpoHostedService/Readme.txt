Framework .Net Core 3.1
IDE Visual Studio 2022

- appsettings.json -> 
	|_ interval: interval in segs of the timer 
	|_ path_csv: path where csv will be located
	|_ Serilog: loggin configuration
	
- program.cs -> Application entry point. A host with information about the settings file, services dependency injection and the log system is configured. One of the services that this host starts is a timer, which will trigger a worker every time interval established in the appsettings file.
- TimeHostedService.cs -> Execute each X seconds the .DoWork() of a service witch implements de interface IWorker
- Implementation 
	|_ DummyWorker.cs -> A dummy implementation for IWorker
	|_ Worker.cs -> Implementation of IWorker for bring the information of the PowerTrades and give those data to the service responsible of generate the csv file
	|_ ReportGen.cs -> Implementation of IReportGen responsible of generate the csv file
- Interfaces
	|_ IWork.cs -> Interface IWorker 
	|_ IReportGen -> Interface IReportGen
