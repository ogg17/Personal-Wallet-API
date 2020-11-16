# Personal-Wallet-API

### ASP.NET Core 3.1 WebApi Application

## Содержание
- [Personal-Wallet-API](#personal-wallet-api)
  * [Возможности](#capabilities)
  * [Базовые структуры](#basic)
  * [Запросы к API](#request)
    + [Запросы GET](#requestget)
      - [Запрос: `GET [URL]/api/users HTTP/1.1`](#requestget-1)
      - [Запрос: `GET [URL]/api/users/[id] HTTP/1.1`](#requestget-2)
    + [Запросы POST](#requestpost)
      - [Запрос: `POST [URL]/api/users/ HTTP/1.1 {UserDto}`](#requestpost-1)
    + [Запросы PUT](#requestput)
      - [Запрос: `PUT [URL]/api/users/[id] HTTP/1.1 {UserDto}`](#requestput-1)
      - [Запрос: `PUT [URL]/api/users/[id]/topup?walletFrom=[wallet from]&value=[value] HTTP/1.1`](#requestput-2)
      - [Запрос: `PUT [URL]/api/users/[id]/withdraw?walletFrom=[wallet from]&value=[value] HTTP/1.1`](#requestput-3)
      - [Запрос: `PUT [URL]/api/users/[id]/transfer?walletFrom=[wallet from]&walletTo=[wallet to]&value=[value] HTTP/1.1`](#requestput-4)
    + [Запросы DELETE](#requestdelete)
      - [Запрос: `DELETE [URL]/api/users/[id] HTTP/1.1`](#requestdelete-1)

## Возможности <a name="capabilities"></a>

PersonalWalletAPI позволяет добавлять, удалять, обновлять пользователей. Также производить операции пополнения, снятия денег с кошелька пользователя. А также операцию перевода с кошелька пользователя на другой кошелек с получением курса валют с публичного API.

## Базовые структуры <a name="basic"></a>
Json шаблон пользователя(класс `UserDto`):
```
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

## Запросы к API <a name="request"></a>
### Запросы GET <a name="requestget"></a>
#### Запрос: `GET [URL]/api/users HTTP/1.1` <a name="requestget-1"></a> <a name="requestget-1"></a>
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

#### Пример:

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
#### Запрос: `GET [URL]/api/users/[id] HTTP/1.1` <a name="requestget-2"></a>

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

#### Пример:

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
### Запросы POST <a name="requestpost"></a>

#### Запрос: `POST [URL]/api/users/ HTTP/1.1 {UserDto}` <a name="requestpost-1"></a>

Позволяет добавить пользователя в систему, принимает объект UserDto.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Server: Kestrel
Content-Length: 0
```

#### Пример:

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
#### Запрос: `PUT [URL]/api/users/[id] HTTP/1.1 {UserDto}`

Позволяет изменять пользователя в системе, принимает объект UserDto.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Content-Length: 0
```

#### Пример:

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
### Запросы PUT <a name="requestput"></a>

#### Запрос: `PUT [URL]/api/users/[id]/topup?walletFrom=[wallet from]&value=[value] HTTP/1.1` <a name="requestput-1"></a>

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

#### Пример:

**Запрос:** `PUT https://localhost:5001/api/users/2/topup?walletFrom=rub&value=10` <a name="requestput-2"></a>

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
#### Запрос: `PUT [URL]/api/users/[id]/withdraw?walletFrom=[wallet from]&value=[value] HTTP/1.1` <a name="requestput-3"></a>

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

#### Пример:

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
#### Запрос: `PUT [URL]/api/users/[id]/transfer?walletFrom=[wallet from]&walletTo=[wallet to]&value=[value] HTTP/1.1` <a name="requestput-4"></a>

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

#### Пример:

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
### Запросы DELETE <a name="requestdelete"></a>

#### Запрос: `DELETE [URL]/api/users/[id] HTTP/1.1` <a name="requestdelete-1"></a>

Удаляет пользователя из системы по `id`.

Ответ:

```
HTTP/1.1 200 OK
Date: [Time] GMT
Server: Kestrel
Content-Length: 0
```

#### Пример:

**Запрос:** `DELETE https://localhost:5001/api/users/2 HTTP/1.1`

**Ответ:**

```
HTTP/1.1 200 OK
Date: Mon, 16 Nov 2020 10:50:43 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
```
