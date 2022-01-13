Console.WriteLine("Programm Startet!!!. Press Ctrl+C to end.");

//Init Display
using I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
using var driver = new Pcf8574(i2c);
using var lcd = new Lcd1602(registerSelectPin: 0,
    enablePin: 2,
    dataPins: new int[] {4, 5, 6, 7},
    backlightPin: 3,
    backlightBrightness: 0.1f,
    readWritePin: 1,
    controller: new GpioController(PinNumberingScheme.Logical, driver));

//Init BME280 Senor
var i2cSettings = new I2cConnectionSettings(1, Bme280.DefaultI2cAddress);
using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
using var bme280 = new Bme280(i2cDevice);

int measurementTime = bme280.GetMeasurementDuration();

int currentLine = 0;
int secondLine = 1;

while (true)
{
    Console.Clear();
    lcd.Clear();
    bme280.SetPowerMode(Bmx280PowerMode.Forced);
    Thread.Sleep(measurementTime);

    bme280.TryReadTemperature(out var tempValue);
    bme280.TryReadHumidity(out var humValue);

    lcd.SetCursorPosition(0, currentLine);
    lcd.Write($"Temp: {tempValue.DegreesCelsius:0.#} C");
    lcd.SetCursorPosition(0, secondLine);
    lcd.Write($"Luft: {humValue.Percent:#.##}%");

    Thread.Sleep(5000);
}