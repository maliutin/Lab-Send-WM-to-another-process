Лабораторная работа сделана в рамках ответа на вопрос на русском Stack Overflow [Как получать HWND_BROADCAST сообщения без создания дополнительного UI?][1].

Лабораторная демонстрирует, как можно пересылать пользовательское Windows сообщение из одного процесса в другой.

Также демонстрируется как можно получать сообщение в объекте типа System.Windows.Forms.Form и System.Windows.Forms.NativeWindow как в основном потоке, так и в отдельном STA потоке.

Дополнительные ссылки:
- [Как получать HWND_BROADCAST сообщения без создания дополнительного UI?][2]
- [Q: How do I create a message-only window from windows forms?][3]
- [TimerNativeWindow][4]

  [1]: https://ru.stackoverflow.com/a/960359/11230
  [2]: https://ru.stackoverflow.com/questions/959870
  [3]: https://stackoverflow.com/a/41173013/1133809
  [4]: https://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/Timer.cs,272
