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
  "Id": "60148db7-8fac-4790-a2fe-937dff5190a3",
  "ExpiresAt": "2023-05-16T16:41:37.4393945+00:00",
  "User": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72959
  },
  "PermissionsType": 2,
  "IsBanned": false
}
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
| deaths               |

### Response: 200

```json
{
  "Users": [
    {
      "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
      "Username": "turecross123",
      "PermissionsType": 0,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 0,
      "PublishedLevelsCount": 0
    },
    {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 0,
      "PublishedLevelsCount": 72959
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
  "FollowerCount": 0,
  "FollowingCount": 0,
  "LikedLevelsCount": 0,
  "PublishedLevelsCount": 0
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
  "FollowerCount": 0,
  "FollowingCount": 0,
  "LikedLevelsCount": 0,
  "PublishedLevelsCount": 0
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Check If You Are Following User

### Method: GET

> ```
> {ip}:10061/api/v1/users/{userId}/following
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
> {ip}:10061/api/v1/users/{userId}/follow
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unfollow User

### Method: POST

> ```
> {ip}:10061/api/v1/users/{userId}/unFollow
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove User

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/user/{userId}/remove
> ```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set User Permissions

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/user/{userId}/setPermissions
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
| deaths             |

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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "Created": "2023-05-13T08:17:25.080862+00:00",
      "Modified": "2023-05-13T08:17:25.080862+00:00",
      "TotalPlays": 1,
      "UniquePlays": 1,
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-30T20:59:00+00:00",
      "Modified": "2019-01-30T20:59:00+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-30T12:08:31+00:00",
      "Modified": "2019-01-30T12:08:31+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-30T06:52:57+00:00",
      "Modified": "2019-01-30T06:52:57+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-30T03:37:58+00:00",
      "Modified": "2019-01-30T03:37:58+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-30T00:02:27+00:00",
      "Modified": "2019-01-30T00:02:27+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-29T23:20:03+00:00",
      "Modified": "2019-01-29T23:20:03+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "s0OGnpd2",
      "Name": "Imported Level (s0OGnpd2)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-29T22:23:10+00:00",
      "Modified": "2019-01-29T22:23:10+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    },
    {
      "Id": "Prb9KzRE",
      "Name": "Imported Level (Prb9KzRE)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72959
      },
      "Created": "2019-01-29T19:42:07+00:00",
      "Modified": "2019-01-29T19:42:07+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Difficulty": 0
    }
  ],
  "Count": 72960
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Has User Completed Level

### Method: GET

> ```
> {ip}:10061/api/v1/levels/{levelId}/completed
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
> {ip}:10061/api/v1/levels/{levelId}/thumbnail
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
> {ip}:10061/api/v1/levels/{levelId}/liked
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
> {ip}:10061/api/v1/levels/{levelId}/like
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unlike Level

### Method: POST

> ```
> {ip}:10061/api/v1/levels/{levelId}/unLike
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Level Metadata

### ⚠️ Moderators (or higher) can do this on any level. People with default permissions can only do this on their own levels.

### Method: POST

> ```
> {ip}:10061/api/v1/levels/{levelId}/edit
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
  "Created": "2019-01-29T22:23:10+00:00",
  "Modified": "2023-05-15T16:45:22.6611146+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "Deaths": 0,
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
> {ip}:10061/api/v1/levels/{levelId}/remove
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
  "Name": "This is a level name",
  "Language": 0,
  "Modified": "2023-05-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "5eLRLJuN",
  "Name": "This is a level name",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
  },
  "Created": "2023-05-02T09:58:43.5485486+00:00",
  "Modified": "2023-05-02T09:58:43.5485486+00:00",
  "TotalPlays": 0,
  "UniquePlays": 0,
  "Likes": 0,
  "Deaths": 0,
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
> {ip}:10061/api/v1/levels/{levelId}/setLevel
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
> {ip]:10061/api/v1/levels/{levelId}/setSound
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
> {ip}:10061/api/v1/levels/{levelId}/setThumbnail
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

## End-point: Get Album

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums/{albumId}
> ```

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

## End-point: Get Album Thumbnail

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/albums/{albumId}/thumbnail
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
> {ip}:10061/api/v1/albums/{albumId}/sidePanel
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
> {ip}:10061/api/v1/albums/{albumId}/completed
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

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/{albumId}/edit
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

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/albums/{albumId}/setThumbnail
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
> {ip}:10061/api/v1/albums/{albumId}/setSidePanel
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
> {ip}:10061/api/v1/albums/{albumId}/remove
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "PlayTime": 93431,
      "Tokens": 41,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-13T08:06:37.2545837+00:00"
    },
    {
      "Id": "e3ebcc4b-4a17-4cb2-8518-62890d77771b",
      "LevelId": "GIlJ9rMi",
      "Position": 2,
      "User": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "PlayTime": 8149,
      "Tokens": 1,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-13T08:17:27.037232+00:00"
    }
  ],
  "Count": 3
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Leaderboard Entry

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/leaderboard/{leaderboardEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Daily Levels {#Daily}

## End-point: Get Daily Level Objects

### Method: GET

> ```
> {url}:10061/api/v1/daily?date=2023-05-14
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
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 0,
          "PublishedLevelsCount": 72960
        },
        "Created": "2019-01-30T03:37:58+00:00",
        "Modified": "2019-01-30T03:37:58+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-15T22:00:00+00:00",
      "Artist": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
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
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 0,
          "PublishedLevelsCount": 72960
        },
        "Created": "2019-01-30T03:37:58+00:00",
        "Modified": "2019-01-30T03:37:58+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-13T22:00:00+00:00",
      "Artist": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
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
  "Date": "2023-05-02"
}
```

### Response: 201

```json
{
  "Id": "43b2ab00-12ca-4ae4-9326-562e697f3fb7",
  "Level": {
    "Id": "GIlJ9rMi",
    "Name": "Titled",
    "Author": {
      "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "Username": "turecross321",
      "PermissionsType": 0,
      "FollowerCount": 0,
      "FollowingCount": 0,
      "LikedLevelsCount": 1,
      "PublishedLevelsCount": 1
    },
    "Created": "2023-05-13T08:17:25.080862+00:00",
    "Modified": "2023-05-13T08:17:25.080862+00:00",
    "TotalPlays": 1,
    "UniquePlays": 1,
    "Likes": 0,
    "Difficulty": 0
  },
  "Date": "2023-05-14T22:00:00+00:00",
  "Artist": {
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

## End-point: Edit Daily Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/daily/{id}/edit
> ```

### Body (**raw**)

```json
{
  "LevelId": "{levelId}",
  "Date": "2023-05-15"
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
    "Created": "2019-01-30T03:37:58+00:00",
    "Modified": "2019-01-30T03:37:58+00:00",
    "TotalPlays": 0,
    "UniquePlays": 0,
    "Likes": 0,
    "Difficulty": 0
  },
  "Date": "2023-05-15T22:00:00+00:00",
  "Artist": {
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
> {ip}:10061/api/v1/daily/{dailyLevelId}/remove
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
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
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
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
> {ip}:10061/api/v1/news/{newsEntryId}/edit
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
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
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
> {ip}:10061/api/v1/news/{newsEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set News Entry Image

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/news/{newsEntryId}/setImage
> ```

### Body

Should be a PNG

### Response: 201

```json

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
      "Id": "1fb26cf7-8db9-4d2f-9bc6-278fa8aa2646",
      "EventType": 0,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
      },
      "ContentLevel": {
        "Id": "5eLRLJuN",
        "Name": "This is a level name",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 0,
          "PublishedLevelsCount": 72960
        },
        "Created": "2023-05-02T09:58:43.5485486+00:00",
        "Modified": "2023-05-02T09:58:43.5485486+00:00",
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
      },
      "ContentUser": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 0
      },
      "ContentUser": {
        "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
        "Username": "turecross123",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 0
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "ContentLeaderboardEntry": {
        "Id": "e3ebcc4b-4a17-4cb2-8518-62890d77771b",
        "LevelId": "GIlJ9rMi",
        "Position": 0,
        "User": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 1,
          "PublishedLevelsCount": 1
        },
        "PlayTime": 8149,
        "Tokens": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-13T08:17:27.037232+00:00"
      },
      "Date": "2023-05-13T08:17:27.0391331+00:00"
    },
    {
      "Id": "364713da-610c-45da-8a64-9d00a75d335a",
      "EventType": 0,
      "Actor": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "ContentLevel": {
        "Id": "GIlJ9rMi",
        "Name": "Titled",
        "Author": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 1,
          "PublishedLevelsCount": 1
        },
        "Created": "2023-05-13T08:17:25.080862+00:00",
        "Modified": "2023-05-13T08:17:25.080862+00:00",
        "TotalPlays": 1,
        "UniquePlays": 1,
        "Likes": 0,
        "Difficulty": 0
      },
      "Date": "2023-05-13T08:17:25.0874232+00:00"
    },
    {
      "Id": "75597e41-c187-4500-8198-ca7f80a2ea76",
      "EventType": 3,
      "Actor": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "ContentLeaderboardEntry": {
        "Id": "ae8ac9ff-f1db-4519-91a0-840b99f19d91",
        "LevelId": "DkVflCl2",
        "Position": -1,
        "User": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 1,
          "PublishedLevelsCount": 1
        },
        "PlayTime": 255397,
        "Tokens": 53,
        "Deaths": 3,
        "Completed": false,
        "Date": "2023-05-13T08:11:50.2710058+00:00"
      },
      "Date": "2023-05-13T08:11:50.2730079+00:00"
    },
    {
      "Id": "2f1328ba-56cf-4398-87f5-273542935adf",
      "EventType": 1,
      "Actor": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "ContentLevel": {
        "Id": "9LxNV8Jk",
        "Name": "Imported Level (9LxNV8Jk)",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 0,
          "PublishedLevelsCount": 72960
        },
        "Created": "2018-10-15T01:10:49+00:00",
        "Modified": "2018-10-15T01:10:49+00:00",
        "TotalPlays": 1,
        "UniquePlays": 1,
        "Likes": 1,
        "Difficulty": 0
      },
      "Date": "2023-05-13T08:06:46.9773965+00:00"
    },
    {
      "Id": "8c7cead5-1bd6-4b61-97fc-a602e12a4a03",
      "EventType": 3,
      "Actor": {
        "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
        "Username": "turecross321",
        "PermissionsType": 0,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 1,
        "PublishedLevelsCount": 1
      },
      "ContentLeaderboardEntry": {
        "Id": "ae0f0fd7-8743-461e-8d4a-56dacc402873",
        "LevelId": "9LxNV8Jk",
        "Position": 0,
        "User": {
          "Id": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
          "Username": "turecross321",
          "PermissionsType": 0,
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 1,
          "PublishedLevelsCount": 1
        },
        "PlayTime": 93431,
        "Tokens": 41,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-05-13T08:06:37.2545837+00:00"
      },
      "Date": "2023-05-13T08:06:37.2582467+00:00"
    }
  ],
  "Count": 8
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Activity

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/activities/{activityId}/remove
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
          "FollowerCount": 0,
          "FollowingCount": 0,
          "LikedLevelsCount": 1,
          "PublishedLevelsCount": 1
        },
        "Created": "2023-05-13T08:17:25.080862+00:00",
        "Modified": "2023-05-13T08:17:25.080862+00:00",
        "TotalPlays": 1,
        "UniquePlays": 1,
        "Likes": 0,
        "Difficulty": 0
      },
      "ContentType": 1,
      "ReasonType": 0,
      "Date": "2023-05-15T17:01:32.9532553+00:00",
      "IssuerId": "00000000-0000-0000-0000-000000000000"
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
> {ip}:10061/api/v1/reports/{reportId}
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
> {ip}:10061/api/v1/reports/{reportId}/remove
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
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 0
      },
      "PunishmentType": 0,
      "Reason": "Still Not Good...",
      "Revoked": false,
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "FollowerCount": 0,
        "FollowingCount": 0,
        "LikedLevelsCount": 0,
        "PublishedLevelsCount": 72960
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
  "Reason": "Not Good...",
  "ExpiresAt": "2023-06-02T09:58:43.5485486+00:00"
}
```

### Response: 201

```json
{
  "Id": "ed43c3e6-a4d6-4d26-ab1e-8c43fcfe2030",
  "Recipient": {
    "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
    "Username": "turecross123",
    "PermissionsType": 0,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 0
  },
  "PunishmentType": 0,
  "Reason": "Not Good...",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
  },
  "IssuedAt": "2023-05-15T19:13:01.052509+00:00",
  "ExpiresAt": "2023-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/{punishmentId}/edit
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
  "Id": "ed43c3e6-a4d6-4d26-ab1e-8c43fcfe2030",
  "Recipient": {
    "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
    "Username": "turecross123",
    "PermissionsType": 0,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 0
  },
  "PunishmentType": 0,
  "Reason": "Still Not Good...",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "FollowerCount": 0,
    "FollowingCount": 0,
    "LikedLevelsCount": 0,
    "PublishedLevelsCount": 72960
  },
  "IssuedAt": "2023-05-15T19:13:01.052509+00:00",
  "ExpiresAt": "2030-06-02T09:58:43.5485486+00:00"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Revoke Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/{punishmentId}/revoke
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃
