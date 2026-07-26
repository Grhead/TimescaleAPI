# TimescaleAPI

## Запуск

```bash
docker compose up --build
```

## API

```
http://localhost:8080
```

## Swagger

```
http://localhost:8080/swagger
```

## Endpoints

| Метод | Endpoint | Описание |
|--------|----------|----------|
| `POST` | `/metrics` | Загрузка и обработка CSV-файла. |
| `GET` | `/results` | Получение результатов с возможностью фильтрации. |
| `GET` | `/values/latest` | Получение последних 10 записей для указанного файла. |
