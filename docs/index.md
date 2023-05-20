# API Documentation

## Information

Every endpoint, unless specified otherwise, requires an `Authorization` header.
The value of `Authorization` should always be your **Session Id**, unless specified otherwise. Your Session Id is the Id which is provided when logging in.

Date is always UTC, and is always formatted like `YYYY-MM-DD`.

| Endpoints                    |
| ---------------------------- |
| [/account](#Account)         |
| [/ip](#Ip)                   |
| [/users](#Users)             |
| [/levels](#Levels)           |
| [/albums](#Albums)           |
| [/leaderboard](#Leaderboard) |
| [/daily](#Daily)             |
| [/news](#News)               |
| [/activities](#Activities)   |
| [/reports](#Reports)         |
| [/punishments](#Punishments) |

# 📁 Account {#Account}

## End-point: Log In

### 🔑 This does not require the `Authorization` header.

### Method: POST

> ```
> {ip}:10061/api/v1/account/login
> ```

### Body (**raw**)

```json
{
  "Email": "{email}",
  "PasswordSha512": "{passwordSha512}"
}
```

### Response: 200

```json
{
  "Id": "0405d98f-59b4-4e4c-a929-d68f7d360b5c",
  "ExpiresAt": "2023-05-17T15:25:35.4060521+00:00",
  "User": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72960,
    "FollowersCount": 0
  },
  "IsBanned": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Log Out

### Method: POST

> ```
> {ip}:10061/api/v1/account/logout
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Send Password Session

### 🔑 This does not require the `Authorization` header.

### Method: POST

> ```
> {ip}:10061/api/v1/account/sendPasswordSession
> ```

### Body (**raw**)

```json
{
  "Email": "{email}"
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Send Email Session

### Method: POST

> ```
> {ip}:10061/api/v1/account/sendEmailSession
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

### 🔑 This requires a Password Session Id instead of an API Session Id in the Authorization header.

> ```
> {ip}:10061/api/v1/account/setPassword
> ```

### Body (**raw**)

```json
{
  "NewPasswordSha512": "{passwordSha512}"
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Email

### 🔑 This requires an Email Session Id instead of an API Session Id in the Authorization header.

### Method: POST

> ```
> {ip}:10061/api/v1/account/setEmail
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

## End-point: Change Username

### Method: POST

> ```
> {ip}:10061/api/v1/account/setUsername
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

## End-point: Send Account Removal Session

### Method: POST

> ```
> {ip}:10061/api/v1/account/sendRemovalSession
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Account

### 🔑 This requires an Account Removal Session Id instead of a normal API Session Id in the Authorization header.

### Method: POST

> ```
> {ip}:10061/api/v1/account/remove
> ```

### Response: 200

```
o7
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 IP Authorization {#Ip}

## End-point: Get Ip Addresses

### Method: GET

> ```
> {ip}:10061/api/v1/ip/addresses
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| authorized |

### Response: 200

```json
{
  "IpAddresses": [
    {
      "IpAddress": "192.168.1.134",
      "Authorized": false,
      "OneTimeUse": false
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Authorize Ip

### Method: POST

> ```
> {ip}:10061/api/v1/ip/authorize
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

## End-point: UnAuthorize Ip

### Method: POST

> ```
> {ip}:10061/api/v1/ip/unAuthorize
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

# 📁 Users {#Users}

## End-point: Get Users

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/users
> ```

### Query Params

| Param       |
| ----------- |
| from        |
| count       |
| search      |
| followedBy  |
| isFollowing |
| descending  |
| orderBy     |

| Can be ordered by:   |
| -------------------- |
| followersCount       |
| followingCount       |
| levelsCount          |
| likedLevelsCount     |
| creationDate         |
| playedLevelsCount    |
| completedLevelsCount |
| totalDeaths          |
| totalPlayTime        |

### Response: 200

```json
{
  "Users": [
    {
      "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
      "Username": "turecross123",
      "PermissionsType": 0,
      "PublishedLevelsCount": 0,
      "FollowersCount": 0
    },
    {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevelsCount": 72960,
      "FollowersCount": 0
    }
  ],
  "Count": 2
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get User with Id

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/users/id/{userId}
> ```

### Response: 200

```json
{
  "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
  "Username": "turecross123",
  "PermissionsType": 0,
  "CreationDate": "2023-05-13T09:35:44.4233373+00:00",
  "FollowersCount": 0,
  "FollowingCount": 0,
  "LikedLevelsCount": 0,
  "PublishedLevelsCount": 0,
  "ActivitiesCount": 6,
  "PlayedLevelsCount": 1,
  "TotalDeaths": 0,
  "TotalPlayTime": 48048
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get User with Username

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/users/username/{username}
> ```

### Response: 200

```json
{
  "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
  "Username": "turecross123",
  "PermissionsType": 0,
  "CreationDate": "2023-05-13T09:35:44.4233373+00:00",
  "FollowersCount": 0,
  "FollowingCount": 0,
  "LikedLevelsCount": 0,
  "PublishedLevelsCount": 0,
  "ActivitiesCount": 6,
  "PlayedLevelsCount": 1,
  "TotalDeaths": 0,
  "TotalPlayTime": 48048
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Check If You Are Following User

### Method: GET

> ```
> {ip}:10061/api/v1/users/id/{userId}/following
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
> {ip}:10061/api/v1/users/id/{userId}/follow
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unfollow User

### Method: POST

> ```
> {ip}:10061/api/v1/users/id/{userId}/unFollow
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove User

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/users/id/{userId}/remove
> ```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set User Permissions

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/users/id/{userId}/setPermissions
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

# 📁 Levels {#Levels}

## End-point: Get Levels

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/levels
> ```

### Query Params

| Param       |
| ----------- |
| from        |
| count       |
| byUser      |
| likedByUser |
| completedBy |
| inAlbum     |
| inDaily     |
| inDailyDate |
| inLastDaily |
| search      |
| descending  |
| orderBy     |

| Can be ordered by: |
| ------------------ |
| creationDate       |
| modificationDate   |
| plays              |
| uniquePlays        |
| likes              |
| fileSize           |
| difficulty         |
| relevance          |
| random             |
| totalDeaths        |
| totalPlayTime      |
| averagePlayTime    |

### Response: 200

```json
{
  "Levels": [
    {
      "Id": "GIlJ9rMi",
      "Name": "Titled",
      "Author": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevelsCount": 1,
        "FollowersCount": 0
      },
      "CreationDate": "2023-05-13T08:17:25.080862+00:00",
      "ModificationDate": "2023-05-13T08:17:25.080862+00:00",
      "TotalPlays": 5,
      "UniquePlays": 2,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "5eLRLJuN",
      "Name": "This is a level name",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2023-05-02T09:58:43.5485486+00:00",
      "ModificationDate": "2023-05-02T09:58:43.5485486+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "zmCmfsRx",
      "Name": "Imported Level (zmCmfsRx)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-30T20:59:00+00:00",
      "ModificationDate": "2019-01-30T20:59:00+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "iAX0VzD6",
      "Name": "Imported Level (iAX0VzD6)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-30T12:08:31+00:00",
      "ModificationDate": "2019-01-30T12:08:31+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "FHpIWamE",
      "Name": "Imported Level (FHpIWamE)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-30T06:52:57+00:00",
      "ModificationDate": "2019-01-30T06:52:57+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "26vQPIlM",
      "Name": "Imported Level (26vQPIlM)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-30T03:37:58+00:00",
      "ModificationDate": "2019-01-30T03:37:58+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "7PwbqWkF",
      "Name": "Imported Level (7PwbqWkF)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-30T00:02:27+00:00",
      "ModificationDate": "2019-01-30T00:02:27+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "C6xTppiy",
      "Name": "Imported Level (C6xTppiy)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-29T23:20:03+00:00",
      "ModificationDate": "2019-01-29T23:20:03+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "s0OGnpd2",
      "Name": "This is an updated level name",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72960,
        "FollowersCount": 0
      },
      "CreationDate": "2019-01-29T22:23:10+00:00",
      "ModificationDate": "2023-05-15T16:45:22.6611146+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    }
  ],
  "Count": 72961
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Has User Completed Level

### Method: GET

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/completed
> ```

### Response: 200

```json
{
  "HasCompleted": true
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level Thumbnail

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/thumbnail
> ```

### Response: 200

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Check If You Have Liked Level

### Method: GET

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/liked
> ```

### Response: 200

```json
{
  "IsLiked": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Like Level

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/like
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unlike Level

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/unLike
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Level Metadata

### ⚠️ Moderators (or higher) can do this on any level. People with default permissions can only do this on their own levels.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/edit
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
  "Id": "s0OGnpd2",
  "Name": "This is an updated level name",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72959
  },
  "CreationDate": "2019-01-29T22:23:10+00:00",
  "ModificationDate": "2023-05-15T16:45:22.6611146+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "TotalDeaths": 0,
  "Language": 4,
  "Difficulty": 0,
  "AlbumIds": [],
  "DailyLevelIds": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Level

### ⚠️ Moderators (or higher) can do this on any level. People with default permissions can only do this on their own levels.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/create
> ```

### Body (**raw**)

```json
{
  "Name": "This is a level name!",
  "Language": 0,
  "ModificationDate": "2023-05-16T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "b8TuPIcA",
  "Name": "This is a level name!",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  },
  "CreationDate": "2023-05-16T09:58:43.5485486+00:00",
  "ModificationDate": "2023-05-16T09:58:43.5485486+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "TotalDeaths": 0,
  "TotalPlayTime": 0,
  "Language": 0,
  "Difficulty": 0,
  "AlbumIds": [],
  "DailyLevelIds": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Level File for Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/setLevel
> ```

### Body

Should be a Sound Shapes Level File

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Sound File for Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip]:10061/api/v1/levels/id/{levelId}/setSound
> ```

### Body (**binary**)

Should be a Sound Shapes Sound File

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Thumbnail for Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/id/{levelId}/setThumbnail
> ```

### Body (**binary**)

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Albums {#Albums}

## End-point: Get Albums

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| descending |
| orderBy    |

| Can be ordered by: |
| ------------------ |
| creationDate       |
| modificationDate   |
| plays              |
| uniquePlays        |
| levelsCount        |
| fileSize           |
| difficulty         |

### Response: 200

```json
{
  "Albums": [
    {
      "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
      "Author": "DJ Khaled the Second",
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

## End-point: Get Album

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums/id/{albumId}
> ```

### Response: 200

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Author": "DJ Khaled the Second",
  "Name": "We Not The Best",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 2,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:59.0157425+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Thumbnail

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/thumbnail
> ```

### Response: 200

### Body (**binary**)

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Side Panel

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/sidePanel
> ```

### Response: 200

### Body (**binary**)

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Completion

### Method: GET

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/completed
> ```

### Response: 200

### Body (**binary**)

```json
{
  "LevelsBeaten": 1,
  "TotalLevels": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Album

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/create
> ```

### Body (**raw**)

```json
{
  "Name": "We The Best",
  "Author": "DJ Khaled",
  "LinerNotes": "<html><h1>This is a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "LevelIds": ["{levelId}", "{otherLevelId}"]
}
```

### Response: 201

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Author": "DJ Khaled",
  "Name": "We The Best",
  "LinerNotes": "<html><h1>This is a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 2,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:32.7037593+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Album

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/edit
> ```

### Body (**raw**)

```json
{
  "Name": "We Not The Best",
  "Author": "DJ Khaled the Second",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "LevelIds": ["{levelId}", "{levelId2}"]
}
```

### Response: 201

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Author": "DJ Khaled the Second",
  "Name": "We Not The Best",
  "LinerNotes": "<html><h1>This is still a Title!</h1><h2>This is a header!</h2><p>This is a paragraph!</p></html>",
  "TotalLevels": 2,
  "CreationDate": "2023-05-02T20:24:32.7037593+00:00",
  "ModificationDate": "2023-05-02T20:24:59.0157425+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Album Thumbnail

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/setThumbnail
> ```

### Body

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Album Side Panel

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/setSidePanel
> ```

### Body

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Album

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/id/{albumId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Leaderboard {#Leaderboard}

## End-point: Get Leaderboard

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/leaderboard
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| onlyBest   |
| completed  |
| byUser     |
| onLevel    |
| descending |
| orderBy    |

| Can be ordered by: |
| ------------------ |
| score              |
| playTime           |
| tokenCount         |
| date               |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "ae8ac9ff-f1db-4519-91a0-840b99f19d91",
      "LevelId": "DkVflCl2",
      "Position": 0,
      "User": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevelsCount": 1,
        "FollowersCount": 0
      },
      "PlayTime": 255397,
      "Tokens": 53,
      "Deaths": 3,
      "Completed": false,
      "Date": "2023-05-13T08:11:50.2710058+00:00"
    },
    {
      "Id": "ae0f0fd7-8743-461e-8d4a-56dacc402873",
      "LevelId": "9LxNV8Jk",
      "Position": 1,
      "User": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevelsCount": 1,
        "FollowersCount": 0
      },
      "PlayTime": 93431,
      "Tokens": 41,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-13T08:06:37.2545837+00:00"
    },
    {
      "Id": "c20de6d1-2e63-4d24-9654-19460b29101f",
      "LevelId": "GIlJ9rMi",
      "Position": 2,
      "User": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "PlayTime": 7033,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-16T15:06:47.2878822+00:00"
    },
    {
      "Id": "6902618d-02b3-41ca-8c16-217b1b1df172",
      "LevelId": "GIlJ9rMi",
      "Position": 3,
      "User": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "PlayTime": 7283,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-16T15:07:19.2745572+00:00"
    },
    {
      "Id": "529d3e20-5ff0-439b-aa91-c5bff73339a2",
      "LevelId": "GIlJ9rMi",
      "Position": 4,
      "User": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "PlayTime": 7299,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-16T15:27:42.9384007+00:00"
    },
    {
      "Id": "db0b653b-43ed-4233-a4e0-cf80ad79574b",
      "LevelId": "GIlJ9rMi",
      "Position": 5,
      "User": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "PlayTime": 7333,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-16T15:27:56.1172208+00:00"
    },
    {
      "Id": "e3ebcc4b-4a17-4cb2-8518-62890d77771b",
      "LevelId": "GIlJ9rMi",
      "Position": 6,
      "User": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevelsCount": 1,
        "FollowersCount": 0
      },
      "PlayTime": 8149,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-13T08:17:27.037232+00:00"
    }
  ],
  "Count": 7
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Leaderboard Entry

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/leaderboard/id/{leaderboardEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Daily Levels {#Daily}

## End-point: Get Daily Level Objects

### Method: GET

> ```
> {url}:10061/api/v1/daily
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| date       |
| lastDate   |
| descending |

### Response: 200

```json
{
  "DailyLevels": [
    {
      "Id": "43b2ab00-12ca-4ae4-9326-562e697f3fb7",
      "Level": {
        "Id": "26vQPIlM",
        "Name": "Imported Level (26vQPIlM)",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevelsCount": 72961,
          "FollowersCount": 0
        },
        "CreationDate": "2019-01-30T03:37:58+00:00",
        "ModificationDate": "2019-01-30T03:37:58+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-15T22:00:00+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      }
    },
    {
      "Id": "c3008f5b-efb6-4403-993f-32ac7008f72c",
      "Level": {
        "Id": "26vQPIlM",
        "Name": "Imported Level (26vQPIlM)",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevelsCount": 72961,
          "FollowersCount": 0
        },
        "CreationDate": "2019-01-30T03:37:58+00:00",
        "ModificationDate": "2019-01-30T03:37:58+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-13T22:00:00+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      }
    }
  ],
  "Count": 2
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Daily Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/daily/create
> ```

### Body (**raw**)

```json
{
  "LevelId": "{levelId}",
  "Date": "2023-05-16"
}
```

### Response: 201

```json
{
  "Id": "1887e8d5-3c87-4e24-8304-0523128195ff",
  "Level": {
    "Id": "b8TuPIcA",
    "Name": "This is a level name!",
    "Author": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevelsCount": 72961,
      "FollowersCount": 0
    },
    "CreationDate": "2023-05-16T09:58:43.5485486+00:00",
    "ModificationDate": "2023-05-16T09:58:43.5485486+00:00",
    "TotalPlays": 0,
    "UniquePlays": 0,
    "Likes": 0,
    "Difficulty": 0
  },
  "Date": "2023-05-15T22:00:00+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  }
}
```

### Response: 201

```json
{
  "Id": "43b2ab00-12ca-4ae4-9326-562e697f3fb7",
  "Level": {
    "Id": "26vQPIlM",
    "Name": "Imported Level (26vQPIlM)",
    "Author": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 0,
      "PublishedLevelsCount": 72960
    },
    "CreationDate": "2019-01-30T03:37:58+00:00",
    "ModificationDate": "2019-01-30T03:37:58+00:00",
    "TotalPlays": 0,
    "UniquePlays": 0,
    "Likes": 0,
    "Difficulty": 0
  },
  "Date": "2023-05-15T22:00:00+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
  }
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Daily Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/daily/id/{dailyLevelId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 News {#News}

## End-point: Get News

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/news
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| descending |
| language   |
| authors    |
| orderBy    |

### Note: `authors` can have multiple User Ids, seperated by commas. So an `actors` query can, for example, be `actors=5503f74b-2620-4103-a892-dcbd99435645,01fb1e88-507c-44a5-a99f-16b6cca2eccd` which would only return news written by those two users.

| Can be ordered by: |
| ------------------ |
| creationDate       |
| modificationDate   |
| characterCount     |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "eab4643e-cc22-4b57-b7e2-3bda7558b534",
      "CreationDate": "2023-05-15T16:53:46.8092128+00:00",
      "ModificationDate": "2023-05-15T16:55:43.3707501+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "Language": "global",
      "Title": "DJ Khaled is still stuck in a tree!",
      "Summary": "This still ain't no joke team!",
      "FullText": "DJ Khaled is still stuck in a tree, and he is losing hope!",
      "Url": "http://djkhaled.com"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create News Entry

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/news/create
> ```

### Body (**raw**)

```json
{
  "Language": "global",
  "Title": "DJ Khaled is stuck in a tree!",
  "Summary": "This ain't no joke team!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

### Response: 201

```json
{
  "Id": "eab4643e-cc22-4b57-b7e2-3bda7558b534",
  "CreationDate": "2023-05-15T16:53:46.8092128+00:00",
  "ModificationDate": "2023-05-15T16:53:46.8093049+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  },
  "Language": "global",
  "Title": "DJ Khaled is stuck in a tree!",
  "Summary": "This ain't no joke team!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit News Entry

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/news/id/{newsEntryId}/edit
> ```

### Body (**raw**)

```json
{
  "Language": "global",
  "Title": "DJ Khaled is still stuck in a tree!",
  "Summary": "This still ain't no joke team!",
  "FullText": "DJ Khaled is still stuck in a tree, and he is losing hope!",
  "Url": "http://djkhaled.com"
}
```

### Response: 201

```json
{
  "Id": "eab4643e-cc22-4b57-b7e2-3bda7558b534",
  "CreationDate": "2023-05-15T16:53:46.8092128+00:00",
  "ModificationDate": "2023-05-15T16:55:43.3707501+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  },
  "Language": "global",
  "Title": "DJ Khaled is still stuck in a tree!",
  "Summary": "This still ain't no joke team!",
  "FullText": "DJ Khaled is still stuck in a tree, and he is losing hope!",
  "Url": "http://djkhaled.com"
}
```

## End-point: Remove News Entry

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/news/id/{newsEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set News Entry Thumbnail

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/news/id/{newsEntryId}/setThumbnail
> ```

### Body

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get News Entry Thumbnail

### ⚠️ This requires administrator permissions.

### Method: GET

> ```
> {ip}:10061/api/v1/news/id/{newsEntryId}/thumbnail
> ```

### Response: 201

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Recent Activity {#Activities}

## End-point: Get Activities

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/activities
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| actors     |
| onUser     |
| onLevel    |
| eventTypes |
| descending |
| orderBy    |

#

### Note: `actors` can have multiple User Ids, seperated by commas. So an `actors` query can, for example, be `actors=5503f74b-2620-4103-a892-dcbd99435645,01fb1e88-507c-44a5-a99f-16b6cca2eccd` which would only return events that those two users did.

#

### Note: `eventTypes` can have multiple values at the same time, seperated by commas. So an `eventType` query can, for example, be `eventTypes=0,1` which would return only `Publish` and `Like` events.

#

| Value | Event Type           |
| ----- | -------------------- |
| 0     | Publish              |
| 1     | Like                 |
| 2     | Follow               |
| 3     | Score Submission     |
| 4     | Account Registration |

| Can be ordered by: |
| ------------------ |
| date               |

### Response: 200

```json
{
  "Activities": [
    {
      "Id": "489012f8-637b-4f5b-b83f-d713b20d1e80",
      "EventType": 0,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "ContentLevel": {
        "Id": "b8TuPIcA",
        "Name": "This is a level name!",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevelsCount": 72961,
          "FollowersCount": 0
        },
        "CreationDate": "2023-05-16T09:58:43.5485486+00:00",
        "ModificationDate": "2023-05-16T09:58:43.5485486+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-16T15:38:48.377909+00:00"
    },
    {
      "Id": "f8b0e054-08d6-4e60-a7ca-10a373605282",
      "EventType": 3,
      "Actor": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "ContentLeaderboardEntry": {
        "Id": "db0b653b-43ed-4233-a4e0-cf80ad79574b",
        "LevelId": "GIlJ9rMi",
        "Position": -1,
        "User": {
          "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
          "Username": "turecross123",
          "PermissionsType": 0,
          "PublishedLevelsCount": 0,
          "FollowersCount": 0
        },
        "PlayTime": 7333,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-16T15:27:56.1172208+00:00"
      },
      "Date": "2023-05-16T15:27:56.1190942+00:00"
    },
    {
      "Id": "afc0e7ca-44af-40a7-bdb5-2311cf3f3ace",
      "EventType": 3,
      "Actor": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "ContentLeaderboardEntry": {
        "Id": "529d3e20-5ff0-439b-aa91-c5bff73339a2",
        "LevelId": "GIlJ9rMi",
        "Position": -1,
        "User": {
          "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
          "Username": "turecross123",
          "PermissionsType": 0,
          "PublishedLevelsCount": 0,
          "FollowersCount": 0
        },
        "PlayTime": 7299,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-16T15:27:42.9384007+00:00"
      },
      "Date": "2023-05-16T15:27:42.9430711+00:00"
    },
    {
      "Id": "7e59bf49-0826-43b9-8804-b2c3938f3909",
      "EventType": 3,
      "Actor": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "ContentLeaderboardEntry": {
        "Id": "6902618d-02b3-41ca-8c16-217b1b1df172",
        "LevelId": "GIlJ9rMi",
        "Position": -1,
        "User": {
          "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
          "Username": "turecross123",
          "PermissionsType": 0,
          "PublishedLevelsCount": 0,
          "FollowersCount": 0
        },
        "PlayTime": 7283,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-16T15:07:19.2745572+00:00"
      },
      "Date": "2023-05-16T15:07:19.2788198+00:00"
    },
    {
      "Id": "c2cba14a-0d76-4f4e-bcd5-7bd4f51c62f2",
      "EventType": 3,
      "Actor": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "ContentLeaderboardEntry": {
        "Id": "c20de6d1-2e63-4d24-9654-19460b29101f",
        "LevelId": "GIlJ9rMi",
        "Position": 0,
        "User": {
          "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
          "Username": "turecross123",
          "PermissionsType": 0,
          "PublishedLevelsCount": 0,
          "FollowersCount": 0
        },
        "PlayTime": 7033,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-16T15:06:47.2878822+00:00"
      },
      "Date": "2023-05-16T15:06:47.2913487+00:00"
    },
    {
      "Id": "1fb26cf7-8db9-4d2f-9bc6-278fa8aa2646",
      "EventType": 0,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "ContentLevel": {
        "Id": "5eLRLJuN",
        "Name": "This is a level name",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevelsCount": 72961,
          "FollowersCount": 0
        },
        "CreationDate": "2023-05-02T09:58:43.5485486+00:00",
        "ModificationDate": "2023-05-02T09:58:43.5485486+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-15T16:45:59.2781181+00:00"
    },
    {
      "Id": "d3c01ce8-b758-4615-8c57-14685e4d8459",
      "EventType": 4,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "ContentUser": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "Date": "2023-05-14T00:14:49.9650531+00:00"
    },
    {
      "Id": "4ad17659-006e-4b36-b319-c7039b8b52c8",
      "EventType": 4,
      "Actor": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "ContentUser": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "Date": "2023-05-13T09:50:49.1702927+00:00"
    },
    {
      "Id": "35aefbdc-e3d8-4f08-8bb4-16583d010d36",
      "EventType": 3,
      "Actor": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevelsCount": 1,
        "FollowersCount": 0
      },
      "ContentLeaderboardEntry": {
        "Id": "e3ebcc4b-4a17-4cb2-8518-62890d77771b",
        "LevelId": "GIlJ9rMi",
        "Position": 1,
        "User": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevelsCount": 1,
          "FollowersCount": 0
        },
        "PlayTime": 8149,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-13T08:17:27.037232+00:00"
      },
      "Date": "2023-05-13T08:17:27.0391331+00:00"
    }
  ],
  "Count": 13
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Activity

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/activities/id/{activityId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Reports {#Reports}

## End-point: Get Reports

### ⚠️ This requires moderator (or higher) permissions.

### Method: GET

> ```
> {ip}:10061/api/v1/reports
> ```

### Query Params

| Param              |
| ------------------ |
| from               |
| count              |
| levelId            |
| userId             |
| leaderboardEntryId |
| contentType        |
| reasonType         |
| descending         |

| Content Type | Reason            |
| ------------ | ----------------- |
| 0            | User              |
| 1            | Level             |
| 2            | Leaderboard Entry |

| Reason Type | Reason        |
| ----------- | ------------- |
| 0           | Mature        |
| 1           | Offensive     |
| 2           | Defamation    |
| 3           | Impersonation |
| 4           | Inappropriate |
| 5           | Other         |

### Response: 200

```json
{
  "Reports": [
    {
      "Id": "15d0e958-1c82-409e-a3e9-1e25bf98d629",
      "ContentLevel": {
        "Id": "GIlJ9rMi",
        "Name": "Titled",
        "Author": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevelsCount": 1,
          "FollowersCount": 0
        },
        "CreationDate": "2023-05-13T08:17:25.080862+00:00",
        "ModificationDate": "2023-05-13T08:17:25.080862+00:00",
        "TotalPlays": 5,
        "UniquePlays": 2,
        "Likes": 0,
        "Difficulty": 0
      },
      "ContentType": 1,
      "ReasonType": 0,
      "Date": "2023-05-15T17:01:32.9532553+00:00",
      "Issuer": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      }
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Report Object

### ⚠️ This requires moderator (or higher) permissions.

### Method: GET

> ```
> {ip}:10061/api/v1/reports/id/{reportId}
> ```

### Response: 200

```json
{
  "Id": "15d0e958-1c82-409e-a3e9-1e25bf98d629",
  "ContentLevel": {
    "Id": "GIlJ9rMi",
    "Name": "Titled",
    "Author": {
      "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "Username": "turecross321",
      "PermissionsType": 0,
      "PublishedLevelsCount": 1,
      "FollowersCount": 0
    },
    "CreationDate": "2023-05-13T08:17:25.080862+00:00",
    "ModificationDate": "2023-05-13T08:17:25.080862+00:00",
    "TotalPlays": 5,
    "UniquePlays": 2,
    "Likes": 0,
    "Difficulty": 0
  },
  "ContentType": 1,
  "ReasonType": 0,
  "Date": "2023-05-15T17:01:32.9532553+00:00",
  "Issuer": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  }
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Submit Report

### 💡 Content Id is a User Id, a Level Id or a Leaderboard Entry Id.

| Content Type | Reason            |
| ------------ | ----------------- |
| 0            | User              |
| 1            | Level             |
| 2            | Leaderboard Entry |

| Reason Type | Reason        |
| ----------- | ------------- |
| 0           | Mature        |
| 1           | Offensive     |
| 2           | Defamation    |
| 3           | Impersonation |
| 4           | Inappropriate |
| 5           | Other         |

### Method: POST

> ```
> {ip}:10061/api/v1/reports/create
> ```

### Body (**raw**)

```json
{
  "ContentType": 1,
  "ContentId": "{levelId}",
  "ReasonType": 1
}
```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Report

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/reports/id/{reportId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Punishments {#Punishments}

## End-point: Get Punishments

### ⚠️ This requires moderator (or higher) permissions.

### Method: GET

> ```
> {ip}:10061/api/v1/punishments
> ```

### Query Params

| Param      |
| ---------- |
| from       |
| count      |
| author     |
| recipient  |
| revoked    |
| descending |

### Response: 200

```json
{
  "Punishments": [
    {
      "Id": "ed43c3e6-a4d6-4d26-ab1e-8c43fcfe2030",
      "Recipient": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "PublishedLevelsCount": 0,
        "FollowersCount": 0
      },
      "PunishmentType": 0,
      "Reason": "Still Not Good...",
      "Revoked": true,
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevelsCount": 72961,
        "FollowersCount": 0
      },
      "IssuedAt": "2023-05-15T19:13:01.052509+00:00",
      "ExpiresAt": "2030-06-02T09:58:43.5485486+00:00"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Punish User

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/create
> ```

| PunishmentType | Punishment |
| -------------- | ---------- |
| 0              | Ban        |

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0,
  "Reason": "NOT GOOD...",
  "ExpiresAt": "2023-06-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "52036622-ffda-460e-b4d6-5f8f78c27790",
  "Recipient": {
    "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
    "Username": "turecross123",
    "PermissionsType": 0,
    "PublishedLevelsCount": 0,
    "FollowersCount": 0
  },
  "PunishmentType": 0,
  "Reason": "NOT GOOD...",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  },
  "IssuedAt": "2023-05-16T15:50:34.7685334+00:00",
  "ExpiresAt": "2023-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/id/{punishmentId}/edit
> ```

| PunishmentType | Punishment |
| -------------- | ---------- |
| 0              | Ban        |

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0,
  "Reason": "Still Not Good...",
  "ExpiresAt": "2030-06-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "52036622-ffda-460e-b4d6-5f8f78c27790",
  "Recipient": {
    "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
    "Username": "turecross123",
    "PermissionsType": 0,
    "PublishedLevelsCount": 0,
    "FollowersCount": 0
  },
  "PunishmentType": 0,
  "Reason": "Still Not Good...",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevelsCount": 72961,
    "FollowersCount": 0
  },
  "IssuedAt": "2023-05-16T15:50:34.7685334+00:00",
  "ExpiresAt": "2030-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Revoke Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/id/{punishmentId}/revoke
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃
