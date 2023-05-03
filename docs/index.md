# API Documentation

# 📁 Collection: Account

## End-point: Log In

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/login
> ```

### Body (**raw**)

```json
{
  "Email": "{email}",
  "PasswordSha512": "{passwordSha512}"
}
```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Id": "ced589a1-2890-4715-a456-e4e2ac58e132",
  "ExpiresAtUtc": "2023-05-03T19:56:49.5955676+00:00",
  "UserId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "Username": "turecross321",
  "PermissionsType": 2
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Send Password Session

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/sendPasswordSession
> ```

### Body (**raw**)

```json
{
  "Email": "{email}"
}
```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Send Email Session

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/sendEmailSession
> ```

### Body (**raw**)

```json
{
  "NewEmail": "{email}"
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Password

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/setPassword
> ```

### Body (**raw**)

```json
{
  "NewPasswordSha512": "{passwordSha512}"
}
```

### 🔑 Authentication apikey

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Email

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/setEmail
> ```

### Body (**raw**)

```json
{
  "NewEmail": "{email}"
}
```

### 🔑 Authentication apikey

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Change Username

### Method: POST

> ```
> http://{ip}:10061/api/v1/account/setUsername
> ```

### Body (**raw**)

```json
{
  "NewUsername": "{newUsername}"
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Collection: IP Authorization

## End-point: Get Unauthorized Ips

### Method: GET

> ```
> http://{ip}:10061/api/v1/ip/unAuthorized
> ```

### Body (**raw**)

```json

```

### Response: 200

```json
{
  "IpAddresses": [
    {
      "IpAddress": "192.168.1.134"
    },
    {
      "IpAddress": "192.168.1.223"
    }
  ]
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Authorize Ip

### Method: POST

> ```
> http://{ip}:10061/api/v1/ip/authorize
> ```

### Body (**raw**)

```json
{
  "IpAddress": "{ipAddress}",
  "OneTimeUse": false
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Authorized Ips

### Method: GET

> ```
> http://{ip}:10061/api/v1/ip/authorized
> ```

### Response: 200

```json
{
  "IpAddresses": [
    {
      "IpAddress": "192.168.1.223",
      "OneTimeUse": false
    }
  ]
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: UnAuthorize Ip

### Method: POST

> ```
> http://{ip}:10061/api/v1/ip/unAuthorize
> ```

### Body (**raw**)

```json
{
  "IpAddress": "{ipAddress}"
}
```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Collection: Moderation

## End-point: Create Album

### Method: POST

> ```
> http://{ip}:10061/api/v1/albums/create
> ```

### Body (**raw**)

```json
{
  "Name": "We The Best",
  "Artist": "DJ Khaled",
  "LinerNotes": "<html><h1>This is a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "LevelIds": ["{levelId}"]
}
```

### Response: 201

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Artist": "DJ Khaled",
  "Name": "We The Best",
  "LinerNotes": "<html><h1>This is a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 1,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:32.7037593+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Album

### Method: POST

> ```
> http://{ip}:10061/api/v1/album/{albumId}/edit
> ```

### Body (**raw**)

```json
{
  "Name": "We Not The Best",
  "Artist": "DJ Khaled the Second",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "LevelIds": ["{levelId}", "{levelId2}"]
}
```

### Response: 201

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Artist": "DJ Khaled the Second",
  "Name": "We Not The Best",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 2,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:59.0157425+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Album Thumbnail

### Method: POST

> ```
> http://{ip}:10061/api/v1/album/{albumId}/setThumbnail
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Album Side Panel

### Method: POST

> ```
> http://{ip}:10061/api/v1/album/{albumId}/setSidePanel
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Album

### Method: POST

> ```
> http://{ip}:10061/api/v1/album/{albumId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Daily Level Objects

### Method: GET

> ```
> http://localhost:10061/api/v1/daily
> ```

### Query Params

| Param | value |
| ----- | ----- |
| from  | 0     |
| count | 10    |

### Response: 200

```json
{
  "DailyLevels": [
    {
      "Id": "f7b43f8a-acb3-45e6-82c6-403ce5c0e0a9",
      "LevelId": "1zyrrn7g",
      "DateUtc": "2023-05-02T09:58:43.5485486+00:00"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Daily Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/daily/create
> ```

### Body (**raw**)

```json
{
  "LevelId": "{levelId}",
  "DateUtc": "2023-05-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "f7b43f8a-acb3-45e6-82c6-403ce5c0e0a9",
  "LevelId": "1zyrrn7g",
  "DateUtc": "2023-05-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Daily Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/daily/{dailyLevelId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/levels/create
> ```

### Body (**raw**)

```json
{
  "Name": "This is a level name",
  "Language": 0,
  "Modified": "2023-05-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "1zyrrn7g",
  "Name": "This is a level name",
  "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "AuthorName": "turecross321",
  "Created": "2023-05-02T09:58:43.5485486+00:00",
  "Modified": "2023-05-02T09:58:43.5485486+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "Deaths": 0,
  "Language": 0,
  "Difficulty": 0,
  "AlbumIds": [],
  "DailyLevelIds": [],
  "CompletedByYou": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Level File for Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/level/{levelId}/setLevel
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Sound File for Level

### Method: POST

> ```
> http://localhost:10061/api/v1/level/abcd1234/setSound
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Thumbnail for Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/level/{levelId}/setThumbnail
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove User

### Method: POST

> ```
> http://{ip}:10061/api/v1/user/{userId}/remove
> ```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set User Permissions

### Method: POST

> ```
> http://{ip}:10061/api/v1/user/{userId}/setPermissions
> ```

### Body (**raw**)

```json
{
  "PermissionsType": 2
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create News Entry

### Method: POST

> ```
> http://{ip}:10061/api/v1/news/create
> ```

### Body (**raw**)

```json
{
  "Language": "global",
  "Title": "BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

### Response: 201

```json
{
  "Id": "e9ecde58-c0c3-4135-ac54-f130a0273757",
  "Date": "2023-05-02T22:24:41.0155527+00:00",
  "Language": "global",
  "Title": "BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit News Entry

### Method: POST

> ```
> http://{ip}:10061/api/v1/news/{newsEntryId}/edit
> ```

### Body (**raw**)

```json
{
  "Language": "global",
  "Title": "VERY BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

### Response: 201

```json
{
  "Id": "e9ecde58-c0c3-4135-ac54-f130a0273757",
  "Date": "2023-05-02T22:24:41.0155527+00:00",
  "Language": "global",
  "Title": "VERY BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set News Entry Image

### Method: POST

> ```
> http://{ip}:10061/api/v1/news/{newsEntryId}/setImage
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove News Entry

### Method: POST

> ```
> http://{ip}:10061/api/v1/news/{newsEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Punish User

### Method: POST

> ```
> http://{ip}:10061/api/v1/punishments/create
> ```

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0, // 0 = Ban
  "Reason": "Not Good...",
  "ExpiresAtUtc": "2023-06-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "c6a71883-90be-42ea-9143-9b6706569063",
  "UserId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "PunishmentType": 0,
  "Reason": "Not Good...",
  "Revoked": false,
  "IssuedAtUtc": "2023-05-02T20:33:54.4159691+00:00",
  "ExpiresAtUtc": "2023-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Punishment

### Method: POST

> ```
> http://{ip}:10061/api/v1/punishment/{punishmentId}/edit
> ```

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0, // 0 = Ban
  "Reason": "Even Worse...",
  "ExpiresAtUtc": "2030-06-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "c6a71883-90be-42ea-9143-9b6706569063",
  "UserId": "02354eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "PunishmentType": 0,
  "Reason": "Even Worse...",
  "Revoked": false,
  "IssuedAtUtc": "2023-05-02T20:33:54.4159691+00:00",
  "ExpiresAtUtc": "2030-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Revoke Punishment

### Method: POST

> ```
> http://{ip}:10061/api/v1/punishment/{punishmentId}/revoke
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Punishments

### Method: GET

> ```
> http://{ip}:10061/api/v1/punishments
> ```

### Query Params

| Param     | value                                |
| --------- | ------------------------------------ |
| from      | 0                                    |
| count     | 10                                   |
| user      | 00000000-0000-0000-0000-000000000000 |
| dismissed | false                                |

### Response: 200

```json
{
  "Punishments": [
    {
      "Id": "c6a71883-90be-42ea-9143-9b6706569063",
      "UserId": "02354eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "PunishmentType": 0,
      "Reason": "Even Worse...",
      "Revoked": false,
      "IssuedAtUtc": "2023-05-02T20:33:54.4159691+00:00",
      "ExpiresAtUtc": "2030-06-02T09:58:43.5485486+00:00"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Report Object

### Method: GET

> ```
> http://{ip}:10061/api/v1/report/{reportId}
> ```

### Response: 200

```json
{
  "Id": "bc40cb6d-78e2-4054-9ec7-973a5d9fac48",
  "ContentId": "1kO92jIEm",
  "ContentType": 0,
  "ReportReasonId": 5,
  "Issued": "2023-05-02T20:42:12.5031374+00:00",
  "IssuerId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Report

### Method: POST

> ```
> http://{ip}:10061/api/v1/report/{reportId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Reports

### Method: GET

> ```
> http://{ip}:10061/api/v1/reports
> ```

### Query Params

| Param     | value     |
| --------- | --------- |
| contentId | {levelId} |

### Response: 200

```json
{
  "Reports": [
    {
      "Id": "bc40cb6d-78e2-4054-9ec7-973a5d9fac48",
      "ContentId": "1kO92jIEm",
      "ContentType": 0,
      "ReportReasonId": 5,
      "Issued": "2023-05-02T20:42:12.5031374+00:00",
      "IssuerId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Leaderboard Entry

### Method: POST

> ```
> http://{ip}:10061/api/v1/leaderboard/{leaderboardEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Collection: Users

## End-point: Get User

### Method: GET

> ```
> http://{ip}:10061/api/v1/user/{userId}
> ```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Id": "02354eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "Username": "OtherRegisteredUser",
  "PermissionsType": 2,
  "FollowerCount": 0,
  "FollowingCount": 0,
  "LikedLevelsCount": 0,
  "PublishedLevelsCount": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Users

### Method: GET

> ```
> http://{ip}:10061/api/v1/users?registered=false
> ```

### Query Params

| Param      | value        |
| ---------- | ------------ |
| from       | 0            |
| count      | 10           |
| descending | true         |
| registered | false        |
| orderBy    | creationDate |

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Users": [
    {
      "Id": "02354eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "Username": "OtherRegisteredUser",
      "PermissionsType": 2,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 0,
      "PublishedLevelsCount": 1
    },
    {
      "Id": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "Username": "turecross321",
      "PermissionsType": 2,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 0,
      "PublishedLevelsCount": 3
    }
  ],
  "Count": 2
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Check If You Are Following User

### Method: GET

> ```
> http://{ip}:10061/api/v1/user/{userId}/following
> ```

### Response: 200

```json
{
  "IsFollowing": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Follow User

### Method: POST

> ```
> http://{ip}:10061/api/v1/user/{userId}/follow
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unfollow User

### Method: POST

> ```
> http://{ip}:10061/api/v1/user/{userId}/unFollow
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Collection: Levels

## End-point: Get Levels

### Method: GET

> ```
> http://{ip}:10061/api/v1/levels
> ```

### Query Params

| Param       | value        |
| ----------- | ------------ |
| from        | 0            |
| count       | 10           |
| descending  | true         |
| orderBy     | creationDate |
| byUser      | {userId}     |
| likedByUser | {userId}     |
| inAlbum     | {albumId}    |
| inDaily     | 2023-05-02   |

### Response: 200

```json
{
  "Levels": [
    {
      "Id": "1kO92jIEm",
      "Name": "Realy God Level...",
      "AuthorId": "02354eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "AuthorName": "OtherRegisteredUser",
      "Created": "2023-05-02T20:41:24.956+00:00",
      "Modified": "2023-05-02T20:41:24.956+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "mXjC41dW",
      "Name": "Titled",
      "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "AuthorName": "turecross321",
      "Created": "2023-05-02T20:04:56.3725701+00:00",
      "Modified": "2023-05-02T20:04:56.3725701+00:00",
      "TotalPlays": 1,
      "UniquePlays": 1,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "NqwU4AYF",
      "Name": "Untitled",
      "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "AuthorName": "turecross321",
      "Created": "2023-05-02T20:02:49.9351882+00:00",
      "Modified": "2023-05-02T20:02:49.9351882+00:00",
      "TotalPlays": 1,
      "UniquePlays": 1,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "1zyrrn7g",
      "Name": "This is an updated level name",
      "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "AuthorName": "turecross321",
      "Created": "2023-05-02T09:58:43.5485486+00:00",
      "Modified": "2023-05-02T20:22:07.936802+00:00",
      "TotalPlays": 1,
      "UniquePlays": 1,
      "Likes": 0,
      "Difficulty": 0
    }
  ],
  "Count": 4
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level

### Method: GET

> ```
> http://{ip}:10061/api/v1/level/{levelId}
> ```

### Response: 200

```json
{
  "Id": "1zyrrn7g",
  "Name": "This is an updated level name",
  "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "AuthorName": "turecross321",
  "Created": "2023-05-02T09:58:43.5485486+00:00",
  "Modified": "2023-05-02T20:22:07.936802+00:00",
  "TotalPlays": 1,
  "UniquePlays": 1,
  "Likes": 0,
  "Deaths": 0,
  "Language": 4,
  "Difficulty": 0,
  "AlbumIds": ["a603d25f-4324-41df-b573-c143679bb7f2"],
  "DailyLevelIds": ["f7b43f8a-acb3-45e6-82c6-403ce5c0e0a9"],
  "CompletedByYou": true
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level Thumbnail

### Method: GET

> ```
> http://{ip}:10061/api/v1/level/{levelId}/thumbnail
> ```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
�PNG

���
IHDR��@�������j�bF���PLTE6\ph8�������8�8��������������%7�C\�������'8$�|����8�q@���%7����������'8$�@�����������%7����������'8$������8�p`���%7���������?��������������%7���������@C34�����������%7���������?;d[�����������%7�x@�������'8$������8�o����%7����������N8�q�A����������%7�Τ�������'8$�X�����������%7���������'8$�����8�n����%7���������'8$�����8�q$���%7���������?(������������%7���������?j=q�����������%7�P����������������8�pD���%7����������'8$�p�����������%7�7�����������?����������%7����������G8��|�����������%7����������>�6������������%7����������?(�����8�p����%7�w��������'8$������������%7����������'8$�������������%7��<�������<#�
�����������%7���������A�(������������%7����������?��������������%7�������������������������%7����������?(�����8�n���%7�3�����=P�@��QIDATx���iPTg��q�nխy�VͭI��̼�;5�L�4�i@6��7��UV���1h��1$�c�&1�Yhdi@�f'Fň��N/Z�������[���l�y�ӧ����p��ӣ�@�*�����
��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��0�a�0@ ���Dڇ�U��h�vv�wٛF����t�<ں�T%5U1[�+�
�.��^w��3ґ��O���1�|C�y�)v\�P�; Y�'��$��K=�A��WY���
��I[�0�2}7j�-���P�-z��h��	i�DQa�@�9u��± ��4/u`�F�J��}^�	$�2_.E땬�JR{��<�A"�sV�G�6��:/��ɑ_fCfC�%����e�\~<��zfF{k�E1�3:�#b���
�����Pk�5�B[��9�u}���
�I_����o���f�uk��9<~���|W߭{{�M
]]��g�~���]�&F��
]�[LU}�g;�n���&���
�����#S#c�l�<1Va*h�:��{���LG�����������Ʈ���֩oL��ɇ�n25v_���}��������{Sow�4�N������H�Էu���]�Ý㓷�F�t�]��?��[��u��<4.x/E�ދ[����?u`ǁ�e������#��9y�}������sX �|��u�>���[�ԛ
%5��?n��`������@��T���<>���)	۹0<���t\mG\m��@Y�ys}��3?�����O¡���?�d{��Z�u��Z�Y�9��ǽ����穮�����c,�6oi��s�j�q�z�yh����X�>�9_�W۱����pvpD�}�C�=�?�òn�SY�y�q�v@���K�oFh3�
�����g;������W&�84|�b�%�;�?�o��ز�����ns��`��isy�!`;�;Ε��N����;!��Í��jO�bF���CF]�
�浖O3�ޏ�>�/�pI��bFHp=������h�ۏ��:atyex9$�Zx'�ԯ�,���|���~�^�����,8B�`���C��FMUR���ZoT���>���/7[!�����<;�g���j����V�7#zoF���\���A�]y����e
<2:��^�괭����6
�&��.��ˍ������?�9�j!�vG����fC�G�{��QVs�T��/	�\[�x�y���T�⿮��Qho/-�s'�V�{`��Ԑ��-m��|b_��ӣ��o?��y���|�ۏ�	��Mכ?���g�H?R&|d�|�'*6�NS����ձ_�A��k�NS����U�n6]�J!���;������\�gO��D�yX������i�z�����1c�+�g��i-i-i�:���r�d�n?F������-��x����M�w3��_�,�vW�	C��s�׮'BoAE��dV���W�sx2!�<|�ͻ���o��l(�\yV`ih`�³�������S{J���I������+�C�N�d<7;��+���i_�\�����ː�z�MI�/���Z���gu`G.�9��tW�A�v~�~%�HP1�s�G�wy�;}���]W�όX.h�j�.�a��s�,7��yW�G��$�X�z�̆")u}�4Z&���x���؎d1��t_K�˂4��M7Z\
���b<w��!Ws@�s�[�0��)eڢ]Q�}h}�ٹ��������17.��2��rr�:��:#KWr@W��z<i��;ޖ����y�C�=������/���u�C�&�ٹ��&��<]�����d2��]��ۦ����UyY�����ϝ9�������ϓ�E��uv�u�Q�?��گ�/`b�s��ߍ��ԑ\�s
�^[X��5��nך5y�t�Ke2fA&�H}9Sk�f��m_=�������$!봅�������PWrP���xN�a}Dn��9 �9;�]�}�_����V(�m��QWr��ܷ�/�э׾`<�LH��׊�؈���Y9v�
�j�種����$Pl������G���7�m�s:�9]�^��f����s5�w2_�����{.w��lWW��G�?�JK��Q���\y�.W��
����֕��u>v��yfn�������XC�ܺ�vaM�v]�A�Vm{�v�N�sCw
N�؀������s�~�:���A�\"Y�;|dgG,Rgf3�K�Ѻ}�?�Ȱ��s����)e�@�2�4�/��`<��]*�	Uz�^6�����%	�A[X��E�^�J�\pu�N_��ߺ��
�I���,0�s���I��8���݌��2��ө������~�:x';��zK�21Q��j���"f�bc�:'�zck�U����;s�y�buf�":R���<a~��K���JNQ%�0�;w��R7����T�i��21�no�4ʄUZ���2�06ٮ~�Ǻ�z�Ve\�<2B��f;�zK�2>��`iC���a���xN�R��ҕ		r��vueb�<B�JM�Iß]<�N>Ն�1�S��)���Y[�N=)i��e\�z��6�yZ�<��XI�/�9uV�21ɲ?��a���Et�:;�z�O�̟�>Fm��B����v��꠯���2��xN��H&��dL�������K��g�^n����`����/
����ҕqq��K&	��msXfI�訯�[],��3/uF�<B��"�N�JIe~^s1�#�o��M�KÂ��!��,�V-
Vg�0/��m]f�s`U��)I��*�i�X$�Щ3����4<���������v���]s������Y��K�a�:=K"
�;��̏����|n��iw�-

����I��dI��4��s����9��ݲ�����7�N�s��� #F;���L)�����ɾ��;���uuO%�beB�\�����K���D���[�s'��eb�L>�W�m�qp�:�9)�G�,�_����>�XK�I��W�'�%��8Y��k��܁m���XSDG*�zۗ��?Y�7��NV�J�y[<�f]2#Z�����#�8g��~wj#���-9�
�Rׇ�+��+��ĥ���x�y3#y��;\���-c�*�m���/Nj�O���ha�2�ȷC�հ*��wK���U���b���K.faw	��=�v��k�_��_����|�x���W�>���O���������α�^��R7!�㯝n���םn�k�oF�^���=^kW�=�o����j������|�|��Ȣ?��a߷V;����׼�N���7^s:
�G���y��q��[���
��B���!`��0�a�0@ ��C���!`��0�a�0@ ��C���!`��V��_����?��>o�e>����IEND�B`�
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Check If You Have Liked Level

### Method: GET

> ```
> http://{ip}:10061/api/v1/level/{levelId}/liked
> ```

### Response: 200

```json
{
  "IsLiked": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Level Metadata

### Method: POST

> ```
> http://{ip}:10061/api/v1/level/{levelId}/edit
> ```

### Body (**raw**)

```json
{
  "Name": "This is an updated level name",
  "Language": 4
}
```

### Response: 201

```json
{
  "Id": "1zyrrn7g",
  "Name": "This is an updated level name",
  "AuthorId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "AuthorName": "turecross321",
  "Created": "2023-05-02T09:58:43.5485486+00:00",
  "Modified": "2023-05-02T20:22:07.936802+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "Deaths": 0,
  "Language": 4,
  "Difficulty": 0,
  "AlbumIds": [],
  "DailyLevelIds": [],
  "CompletedByYou": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Like Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/level/{levelId}/like
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unlike Level

### Method: POST

> ```
> http://{ip}:10061/api/v1/level/{levelId}/unLike
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Level

### Method: POST

> ```
> http://localhost:10061/api/v1/level/abcd1234/remove
> ```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Collection: Albums

## End-point: Get Album

### Method: GET

> ```
> http://{ip}:10061/api/v1/album/{albumId}
> ```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Artist": "DJ Khaled the Second",
  "Name": "We Not The Best",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 2,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:59.0157425+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Albums

### Method: GET

> ```
> http://{ip}:10061/api/v1/albums
> ```

### Query Params

| Param      | value        |
| ---------- | ------------ |
| from       | 0            |
| count      | 10           |
| descending | true         |
| orderBy    | creationDate |

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Albums": [
    {
      "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
      "Artist": "DJ Khaled the Second",
      "Name": "We Not The Best",
      "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
      "TotalLevels": 2,
      "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
      "ModificationDate": "2023-05-02T20:24:59.0157425+00:00"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Thumbnail

### Method: GET

> ```
> http://{ip}:10061/api/v1/album/{albumId}/thumbnail
> ```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Side Panel

### Method: GET

> ```
> http://{ip}:10061/api/v1/album/{albumId}/sidePanel
> ```

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Submit Report

### Method: POST

> ```
> http://{ip}:10061/api/v1/reports/create
> ```

### Body (**raw**)

```json
{
  "ContentId": "{levelId}", // Can be either a level id or a user id
  "ReportReasonId": 0 // 0 = Mature, 1 = Offensive, 3 = Defamation, 4 = Impersonation, 5 = Inappropriate Username, 6 = Other
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Leaderboard

### Method: GET

> ```
> http://{ip}:10061/api/v1/leaderboard?onlyBest=true&completed=true&orderBy=score&onLevel={levelId}
> ```

### Query Params

| Param      | value     |
| ---------- | --------- |
| from       | 0         |
| count      | 10        |
| descending | false     |
| onlyBest   | true      |
| completed  | true      |
| orderBy    | score     |
| byUser     | {userId}  |
| onLevel    | {levelId} |

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "36ad7aee-99b4-4b97-b418-e3738cde488c",
      "LevelId": "1zyrrn7g",
      "Position": 0,
      "UserId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
      "Username": "turecross321",
      "PlayTime": 6599,
      "Tokens": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-02T20:44:55.3545193+00:00"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get News

### Method: GET

> ```
> http://{ip}:10061/api/v1/news
> ```

### Query Params

| Param      | value  |
| ---------- | ------ |
| from       | 0      |
| count      | 10     |
| descending | true   |
| orderBy    | date   |
| language   | global |

### 🔑 Authentication noauth

| Param | value | Type |
| ----- | ----- | ---- |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "a2e4cf92-4d1c-4315-b88d-8fdb461eca31",
      "Date": "2023-05-02T22:39:35.303+00:00",
      "Language": "global",
      "Title": "RELATIVELY BREAKING NEWS",
      "Summary": "A man has fallen into the river in Lego City!",
      "FullText": "A man has fallen into the river in Lego City! Start the new rescue helicopter. HEY! Build the helicopter and off to the rescue. Prepare the lifeline, lower the stretcher, and make the rescue. The new Emergency Collection from Lego City!",
      "Url": "https://LEGO.com"
    },
    {
      "Id": "e9ecde58-c0c3-4135-ac54-f130a0273757",
      "Date": "2023-05-02T22:38:17.6127246+00:00",
      "Language": "global",
      "Title": "VERY BREAKING NEWS",
      "Summary": "DJ Khaled is stuck in a tree!",
      "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
      "Url": "http://djkhaled.com"
    }
  ],
  "Count": 2
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

---

Powered By: [postman-to-markdown](https://github.com/bautistaj/postman-to-markdown/)
