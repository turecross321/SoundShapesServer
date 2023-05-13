# API Documentation

## Information

Every endpoint, unless specified otherwise, requires an `Authorization` header.
The value of `Authorization` should always be your **Session Id**, unless specified otherwise. Your Session Id is the Id which is provided when logging in.

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
  "Id": "ced589a1-2890-4715-a456-e4e2ac58e132",
  "ExpiresAtUtc": "2023-05-03T19:56:49.5955676+00:00",
  "UserId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c",
  "Username": "turecross321",
  "PermissionsType": 2
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

## End-point: Get User

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/users/{userId}
> ```

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

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}:10061/api/v1/levels/{levelId}
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
  "DailyLevelIds": ["f7b43f8a-acb3-45e6-82c6-403ce5c0e0a9"]
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
> {url}:10061/api/v1/daily?date=2023-05-02
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
      "Id": "2676b5ce-fb5e-44f0-86a5-b4ddaf441673",
      "Level": {
        "Id": "2kM0HrBQ",
        "Name": "Imported Level",
        "AuthorId": "7e751482-8228-4e90-97f4-2b61e80687f5",
        "AuthorName": "Server",
        "Created": "2023-05-07T23:02:58.9952097+00:00",
        "Modified": "2023-05-07T23:02:58.9952097+00:00",
        "TotalPlays": 3,
        "UniquePlays": 1,
        "Likes": 1,
        "Difficulty": 1.6
      },
      "DateUtc": "2023-05-10T22:00:00+00:00"
    }
  ],
  "Count": 1
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
  "DateUtc": "2023-05-02"
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
  "DateUtc": "2023-05-12"
}
```

### Response: 201

```json
{
  "Id": "f7b43f8a-acb3-45e6-82c6-403ce5c0e0a9",
  "LevelId": "1zyrrn7g",
  "DateUtc": "2023-05-12T09:58:43.5485486+00:00"
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

### Note: `authors` can have multiple User Ids, seperated by commas. So an actors query can, for example, be `actors=5503f74b-2620-4103-a892-dcbd99435645,01fb1e88-507c-44a5-a99f-16b6cca2eccd` which would only return news written by those two users.

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
      "Id": "a2e4cf92-4d1c-4315-b88d-8fdb461eca31",
      "CreationDate": "2023-05-02T22:39:35.303+00:00",
      "ModificationDate": "2023-05-02T22:39:35.303+00:00",
      "AuthorUsername": "turecross123",
      "AuthorId": "68c8624a-85d5-457f-b109-9942ade2ee39",
      "Language": "global",
      "Title": "RELATIVELY BREAKING NEWS",
      "Summary": "A man has fallen into the river in Lego City!",
      "FullText": "A man has fallen into the river in Lego City! Start the new rescue helicopter. HEY! Build the helicopter and off to the rescue. Prepare the lifeline, lower the stretcher, and make the rescue. The new Emergency Collection from Lego City!",
      "Url": "https://LEGO.com"
    }
    {
      "Id": "e9ecde58-c0c3-4135-ac54-f130a0273757",
      "CreationDate": "2023-05-02T22:38:17.6127246+00:00",
      "ModificationDate": "2023-05-02T22:38:17.6127246+00:00",
      "AuthorUsername": "turecross123",
      "AuthorId": "68c8624a-85d5-457f-b109-9942ade2ee39",
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
  "CreationDate": "2023-05-02T22:24:41.0155527+00:00",
  "ModificationDate": "2023-05-02T22:24:41.0155527+00:00",
  "AuthorUsername": "turecross123",
  "AuthorId": "68c8624a-85d5-457f-b109-9942ade2ee39",
  "Language": "global",
  "Title": "BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
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
  "CreationDate": "2023-05-02T22:24:41.0155527+00:00",
  "ModificationDate": "2023-05-03T22:24:41.0155527+00:00",
  "AuthorUsername": "turecross123",
  "AuthorId": "68c8624a-85d5-457f-b109-9942ade2ee39",
  "Language": "global",
  "Title": "VERY BREAKING NEWS",
  "Summary": "DJ Khaled is stuck in a tree!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
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

### Note: `actors` can have multiple User Ids, seperated by commas. So an actors query can, for example, be `actors=5503f74b-2620-4103-a892-dcbd99435645,01fb1e88-507c-44a5-a99f-16b6cca2eccd` which would only return events that those two users did.

#

### Note: `eventTypes` can have multiple values at the same time, seperated by commas. So a eventType query can, for example, be `eventTypes=0,1` which would return only `Publish` and `Like` events.

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
      "Id": "35aefbdc-e3d8-4f08-8bb4-16583d010d36",
      "EventType": 3,
      "ActorId": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "ActorUsername": "turecross321",
      "ContentLeaderboardEntryId": "e3ebcc4b-4a17-4cb2-8518-62890d77771b",
      "Date": "2023-05-13T08:17:27.0391331+00:00"
    },
    {
      "Id": "364713da-610c-45da-8a64-9d00a75d335a",
      "EventType": 0,
      "ActorId": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "ActorUsername": "turecross321",
      "ContentLevelId": "GIlJ9rMi",
      "ContentLevelName": "Titled",
      "Date": "2023-05-13T08:17:25.0874232+00:00"
    },
    {
      "Id": "75597e41-c187-4500-8198-ca7f80a2ea76",
      "EventType": 3,
      "ActorId": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "ActorUsername": "turecross321",
      "ContentLeaderboardEntryId": "ae8ac9ff-f1db-4519-91a0-840b99f19d91",
      "Date": "2023-05-13T08:11:50.2730079+00:00"
    },
    {
      "Id": "2f1328ba-56cf-4398-87f5-273542935adf",
      "EventType": 1,
      "ActorId": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "ActorUsername": "turecross321",
      "ContentLevelId": "9LxNV8Jk",
      "ContentLevelName": "Imported Level (9LxNV8Jk)",
      "Date": "2023-05-13T08:06:46.9773965+00:00"
    },
    {
      "Id": "8c7cead5-1bd6-4b61-97fc-a602e12a4a03",
      "EventType": 3,
      "ActorId": "4db810b2-b68b-4034-a851-a20f4cb5d8e2",
      "ActorUsername": "turecross321",
      "ContentLeaderboardEntryId": "ae0f0fd7-8743-461e-8d4a-56dacc402873",
      "Date": "2023-05-13T08:06:37.2582467+00:00"
    }
  ],
  "Count": 5
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

| Param       |
| ----------- |
| from        |
| count       |
| contentId   |
| contentType |
| descending  |

### Response: 200

```json
{
  "Reports": [
    {
      "Id": "bc40cb6d-78e2-4054-9ec7-973a5d9fac48",
      "ContentId": "1kO92jIEm",
      "ContentType": "level",
      "ReportReasonId": 5,
      "Issued": "2023-05-02T20:42:12.5031374+00:00",
      "IssuerId": "27110eaf-f805-4ce4-afe4-cf98d9ca3d0c"
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

### 💡 Content Id is either a Level Id or a User Id.

| Reason Id | Reason                 |
| --------- | ---------------------- |
| 0         | Mature                 |
| 1         | Offensive              |
| 2         | Defamation             |
| 3         | Impersonation          |
| 4         | Inappropriate Username |
| 5         | Other                  |

### Method: POST

> ```
> {ip}:10061/api/v1/reports/create
> ```

### Body (**raw**)

```json
{
  "ContentId": "{levelId}",
  "ReportReasonId": 0
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
| forUser    |
| byUser     |
| revoked    |
| descending |

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

## End-point: Punish User

### ⚠️ This requires moderator (or higher) permissions.

| PunishmentType | Punishment |
| -------------- | ---------- |
| 0              | Ban        |

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/create
> ```

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0,
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

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/{punishmentId}/edit
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

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}:10061/api/v1/punishments/{punishmentId}/revoke
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃
