#Personal-Wallet-API

###ASP.NET Core 3.1 WebApi Application

##Содержание
[TOC]

##Возможности
PersonalWalletAPI позволяет добавлять, удалять, обновлять пользователей. Также производить операции пополнения, снятия денег с кошелька пользователя. А также операцию перевода с кошелька пользователя на другой кошелек с получением курса валют с публичного API.

##Базовые структуры
Json шаблон пользователя(класс `UserDto`):
```Json
{
  "userId": [int ID],
  "name": [string name],
  "wallets": [
    {
      "type": [string type],
      "value": [double value]
    }, //...    
  ]
}
```

##Запросы к API
###Запросы GET
####Запрос: `GET [URL]/api/users HTTP/1.1`
Получение всех пользователей в системе. 

Ответ:

```
headers:
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

body: 
Json[List<UserDto>]
```

####Пример:

**Запрос:** `GET https://localhost:5001/api/users/ HTTP/1.1`

**Ответ:**

Headers:
```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 09:50:06 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
Body:
```Json
[
  {
    "userId": 1,
    "name": "Default",
    "wallets": [
      {
        "type": "Rub",
        "value": 100
      },
      {
        "type": "Usd",
        "value": 10
      },
      {
        "type": "Eur",
        "value": 10
      }
    ]
  },
  {
    "userId": 2,
    "name": "Default",
    "wallets": [
      {
        "type": "Rub",
        "value": 100
      },
      {
        "type": "Usd",
        "value": 10
      },
      {
        "type": "Eur",
        "value": 10
      }
    ]
  }
]
```

---
####Запрос: `GET [URL]/api/users/[id] HTTP/1.1`

Получение пользователя по `id`.

Ответ:

```
headers:
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

body: 
Json{UserDto}
```

####Пример:

**Запрос:** `GET https://localhost:5001/api/users/2 HTTP/1.1`

**Ответ:**

Headers:
```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 09:50:06 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
Body:
```Json
{
  "userId": 2,
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 100
    },
    {
      "type": "Usd",
      "value": 10
    },
    {
      "type": "Eur",
      "value": 10
    }
  ]
}
```

---
####Запрос: `POST [URL]/api/users/ HTTP/1.1 {UserDto}`

Позволяет добавить пользователя в систему, принимает объект UserDto.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Server: Kestrel
Content-Length: 0
```

####Пример:

**Запрос:** `POST https://localhost:5001/api/users HTTP/1.1`

**Body:**
```Json
{
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 100
    }
  ]
}
```

**Ответ:**

```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:31:42 GMT
Server: Kestrel
Content-Length: 0
```

`id` пользователя определяется автоматически.

---
####Запрос: `PUT [URL]/api/users/[id] HTTP/1.1 {UserDto}`

Позволяет изменять пользователя в системе, принимает объект UserDto.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Content-Length: 0
```

####Пример:

**Запрос:** `PUT https://localhost:5001/api/users/2 HTTP/1.1`

**Body:**
```Json
{
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 100
    }
  ]
}
```

**Ответ:**

```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:50:43 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Content-Length: 0
```

---
####Запрос: `PUT [URL]/api/users/[id]/topup?walletFrom=[wallet from]&value=[value] HTTP/1.1`

Пополнение выбранного кошелька пользователя.

Ответ:

```
headers:
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

body: 
Json{UserDto}
```

####Пример:

**Запрос:** `PUT https://localhost:5001/api/users/2/topup?walletFrom=rub&value=10`

**Ответ:**

Headers:
```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:56:27 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
Body:
```Json
{
  "userId": 2,
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 110
    }
  ]
}
```

---
####Запрос: `PUT [URL]/api/users/[id]/withdraw?walletFrom=[wallet from]&value=[value] HTTP/1.1`

Cнятие денег с выбранного кошелька пользователя.

Ответ:

```
headers:
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

body: 
Json{UserDto}
```

####Пример:

**Запрос:** `PUT https://localhost:5001/api/users/2/withdraw?walletFrom=rub&value=50`

**Ответ:**

Headers:
```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:57:12 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
Body:
```Json
{
  "userId": 2,
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 60
    }
  ]
}
```

Нельзя снять больше денег чем есть на кошельке пользователя.

---
####Запрос: `PUT [URL]/api/users/[id]/transfer?walletFrom=[wallet from]&walletTo=[wallet to]&value=[value] HTTP/1.1`

Перевод валюты из одного кошелька пользователя в другой. Курс валют берется из публичного API

Ответ:

```
headers:
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

body: 
Json{UserDto}
```

####Пример:

**Запрос:** `PUT https://localhost:5001/api/users/3/transfer?walletFrom=rub&walletTo=eur&value=10`

**Ответ:**

Headers:
```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:58:57 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
Body:
```Json
{
  "userId": 3,
  "name": "Default",
  "wallets": [
    {
      "type": "Rub",
      "value": 110
    },
    {
      "type": "Usd",
      "value": 10
    },
    {
      "type": "Eur",
      "value": 10.109156839822163
    }
  ]
}
```

---
####Запрос: `DELETE [URL]/api/users/[id] HTTP/1.1`

Удаляет пользователя из системы по `id`.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Server: Kestrel
Content-Length: 0
```

####Пример:

**Запрос:** `DELETE https://localhost:5001/api/users/2 HTTP/1.1`

**Ответ:**

```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:50:43 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
```
