CPS - Control Power Status - Контроль состояния питания.
Платформа: Windows 10/11 .net 6.0 С#.
Тип: условный - WinForms, реальный - скрытый процесс без UI.
Функционал: Автоматическое оповещение об низком и высоком уровне заряда батареи.
Параметры: settings.ini
%: максимальный и минимальный процент заряда в абсолютной величине;
Sound: названия файлов для разных уровней, формат только имя без расширения, поддерживает только wav,
файл должен быть в папке "sound" возле исполняемого файла "CPS.exe";
Key: горячие ключи, 
	EXIT - параметр завершения работы программы;
	REFRESH - чтение новых параметров из ini файла, а также перезагрузка аудиодвижка.
	Работа зависит от параметра AUTOSWITCH: =1 один раз, =0 примерно каждые полчаса работы программы CPS;
	AUTOSWITCH - автоматическое изменение значения ключей EXIT и REFRESH на 0, после одного срабатывания данных ключей.
===========================================================================================================================
CPS - Control Power Status.
Platform: Windows 10/11 .net 6.0 C#.
Type: conditional - WinForms, real - hidden process without UI.
Functionality: Automatic notification of low and high battery levels.
Parameters: settings.ini
%: the maximum and minimum percentage of charge in absolute value;
Sound: file names for different levels, the format is only a name without an extension, supports only wav,
the file must be in the "sound" folder next to the executable file "CPS.exe ";
Key: hot keys, 
	EXIT - the program shutdown parameter;
	REFRESH - reading new parameters from the ini file, as well as restarting the audio engine.
	The operation depends on the AUTOSWITCH parameter: =1 once, =0 approximately every half hour of the CPS program;
	AUTOSWITCH - automatically changes the value of the EXIT and REFRESH keys to 0, after one activation of these keys.
