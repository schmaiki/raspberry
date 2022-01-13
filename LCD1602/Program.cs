Console.WriteLine("Displaying current time. Press Ctrl+C to end.");

using I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
using var driver = new Pcf8574(i2c);
using var lcd = new Lcd1602(registerSelectPin: 0,
    enablePin: 2,
    dataPins: new int[] {4, 5, 6, 7},
    backlightPin: 3,
    backlightBrightness: 0.1f,
    readWritePin: 1,
    controller: new GpioController(PinNumberingScheme.Logical, driver));
int currentLine = 0;

lcd.Clear();
lcd.SetCursorPosition(0, currentLine);
lcd.Write("Uhrzeit:");

while (true)
{
    lcd.SetCursorPosition(0, 1);
    lcd.Write(DateTime.Now.ToShortTimeString());
}