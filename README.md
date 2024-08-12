# TestHook (Blazor Server)

Этот проект реализует Blazor Server, который взаимодействует с SignalR для получения обновлений от веб-хуков. Он служит клиентом, который отображает данные, полученные от веб-хуков.

## Функциональность

- Получение данных через веб-хук и отображение на странице Blazor.
- Использование SignalR для двусторонней связи между сервером и клиентом.
- Взаимодействие с сервисами для подписки на веб-хуки и их получение.

## Структура проекта

- **Controllers**: `TestHookController` принимает данные через HTTP POST и передает их через SignalR.
- **Hubs**: `UpdateHub` для отправки обновлений всем клиентам.
- **Pages**: `PageRazorExample` - страница, отображающая полученные данные.
- **Services**: `HookService` и `SubscriptionService` для управления веб-хуками.

## Установка и запуск

1. Убедитесь, что у вас установлен .NET 6.0 или выше.
2. Клонируйте репозиторий: `git clone <repository-url>`.
3. Перейдите в директорию проекта: `cd TestHook`.
4. Установите зависимости и запустите проект: 
   ```bash
   dotnet restore
   dotnet run
   ```
5. Откройте браузер и перейдите по адресу https://localhost:..../planning
   
## Конфигурация

- URL для подключения к SignalR hub: https://localhost:7006/planninghub.
- URL для отправки данных веб-хуком: https://localhost:7052/api/TestHook/TestWebHook.

  ## Использование

После запуска приложение автоматически подключится к SignalR hub и начнет отображать данные, полученные от веб-хука на странице /planning
