# AxpoHostedService
Prueba técnica AXPO

Framework .Net Core 3.1 <br>
IDE Visual Studio 2022


- [appsettings.json](AxpoHostedService/appsettings.json):
	* **interval**: interval in segs of the timer 
	* **path_csv**: path where csv will be located
	* **Serilog**: loggin configuration

- [program.cs](AxpoHostedService/program.cs): Application entry point. A host with information about the settings file, services dependency injection and the log system is configured. One of the services that this host starts is a timer, which will trigger a worker every time interval established in the appsettings file.
- [TimeHostedService.cs](AxpoHostedService/TimeHostedService.cs): Execute each X seconds the .DoWork() of a service witch implements de interface IWorker
- **Implementation** 
	* [DummyWorker.cs](AxpoHostedService/Implementation/DummyWorker.cs): A dummy implementation for IWorker
	* [Worker.cs](AxpoHostedService/Implementation/Worker.cs): Implementation of IWorker for bring the information of the PowerTrades and give those data to the service responsible of generate the csv file
	* [ReportGen.cs](AxpoHostedService/Implementation/ReportGen.cs): Implementation of IReportGen responsible of generate the csv file
- **Interfaces**
	* [IWork.cs](AxpoHostedService/Interfaces/IWork.cs): Interface IWorker 
	* [IReportGen](AxpoHostedService/Interfaces/IReportGen.cs): Interface IReportGen
