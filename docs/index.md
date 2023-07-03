# API Documentation

## Information

Every endpoint, unless specified otherwise, requires an `Authorization` header.
The value of `Authorization` should always be your **Session Id**, unless specified otherwise. Your Session Id is the Id which is provided when logging in.

Date is always represented in unix seconds.

| Endpoints                        |
| -------------------------------- |
| [/account](#Account)             |
| [/ip](#Ip)                       |
| [/users](#Users)                 |
| [/levels](#Levels)               |
| [/albums](#Albums)               |
| [/leaderboard](#Leaderboard)     |
| [/daily](#Daily)                 |
| [/news](#News)                   |
| [/events](#Events)               |
| [/reports](#Reports)             |
| [/punishments](#Punishments)     |
| [/communityTabs](#CommunityTabs) |

# 📁 Account {#Account}

## End-point: Log In

### 🔑 This does not require the `Authorization` header.

### Method: POST

> ```
> {ip}/api/v1/account/logIn
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
  "Id": "41fc700c-2676-4be1-b992-8cf870068cf8",
  "CreationDate": 1688419810,
  "ExpiryDate": 1688506210,
  "User": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "ActivePunishments": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Log Out

### Method: POST

> ```
> {ip}/api/v1/account/logOut
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Send Password Session

### 🔑 This does not require the `Authorization` header.

### Method: POST

> ```
> {ip}/api/v1/account/sendPasswordSession
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
> {ip}/api/v1/account/sendEmailSession
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Password

### Method: POST

### 🔑 This requires a Password Session Id instead of an API Session Id in the Authorization header.

> ```
> {ip}/api/v1/account/setPassword
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
> {ip}/api/v1/account/setEmail
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
> {ip}/api/v1/account/setUsername
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
> {ip}/api/v1/account/sendRemovalSession
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Account

### 🔑 This requires an Account Removal Session Id instead of a normal API Session Id in the Authorization header.

### Method: POST

> ```
> {ip}/api/v1/account/remove
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
> {ip}/api/v1/ip
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
      "IpAddress": "192.168.1.1",
      "Authorized": false,
      "OneTimeUse": false,
      "CreationDate": 1688408896,
      "ModificationDate": 1688408896
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Authorize Ip

### Method: POST

> ```
> {ip}/api/v1/ip/authorize
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

## End-point: Remove Authorized IP

### Method: POST

> ```
> {ip}/api/v1/ip/address/{ipAddress}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Users {#Users}

## End-point: Get Users

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/users
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

| Can be ordered by: |
| ------------------ |
| followers          |
| following          |
| publishedLevels    |
| likedLevels        |
| creationDate       |
| playedLevels       |
| completedLevels    |
| totalDeaths        |
| totalPlayTime      |
| lastGameLogin      |
| events             |

### Response: 200

```json
{
  "Users": [
    {
      "Id": "d95980b4-ba6b-4c68-93ca-1a2c710ef419",
      "Username": "jvyden420",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 0
    },
    {
      "Id": "3978f174-4e76-4342-b48c-95665563a8ca",
      "Username": "FlyoGang",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 0
    },
    {
      "Id": "df197af6-e901-465a-bd03-cef2b851b368",
      "Username": "Jazzkha11",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 3
    },
    {
      "Id": "7d6ae6ce-ec54-4efa-a429-3805752fbff7",
      "Username": "CatgirlFishing",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 3
    },
    {
      "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
      "Username": "Grrheheheha",
      "PermissionsType": 0,
      "PublishedLevels": 1,
      "Followers": 3
    },
    {
      "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
      "Username": "turecross321",
      "PermissionsType": 0,
      "PublishedLevels": 4,
      "Followers": 3
    },
    {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevels": 72932,
      "Followers": 1
    }
  ],
  "Count": 7
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get User with Id

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/users/id/{userId}
> ```

### Response: 200

```json
{
  "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
  "Username": "turecross321",
  "PermissionsType": 0,
  "CreationDate": 1685292677,
  "LastGameLogin": 1687040679,
  "LastEventDate": 1686227241,
  "Followers": 3,
  "Following": 3,
  "LikedLevels": 6,
  "QueuedLevels": 1,
  "PublishedLevels": 4,
  "Events": 140,
  "PlayedLevels": 24,
  "TotalDeaths": 607,
  "TotalPlayTime": 8671055
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get User with Username

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/users/username/{username}
> ```

### Response: 200

```json
{
  "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
  "Username": "turecross321",
  "PermissionsType": 0,
  "CreationDate": 1685292677,
  "LastGameLogin": 1687040679,
  "LastEventDate": 1686227241,
  "Followers": 3,
  "Following": 3,
  "LikedLevels": 6,
  "QueuedLevels": 1,
  "PublishedLevels": 4,
  "Events": 140,
  "PlayedLevels": 24,
  "TotalDeaths": 607,
  "TotalPlayTime": 8671055
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get User : User Relation

### Method: GET

> ```
> {ip}/api/v1/users/id/{recipientId}/users/id/{actorId}
> ```

### Response: 200

```json
{
  "Following": false,
  "Followed": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Follow User

### Method: POST

> ```
> {ip}/api/v1/users/id/{userId}/follow
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unfollow User

### Method: POST

> ```
> {ip}/api/v1/users/id/{userId}/unFollow
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove User

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/users/id/{userId}/remove
> ```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set User Permissions

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/users/id/{userId}/setPermissions
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
> {ip}/api/v1/levels
> ```

### Query Params

| Param           |
| --------------- |
| from            |
| count           |
| createdBy       |
| likedBy         |
| queuedBy        |
| likedOrQueuedBy |
| completedBy     |
| inAlbum         |
| inDaily         |
| inDailyDate     |
| inLastDaily     |
| search          |
| bpm             |
| transposeValue  |
| scaleIndex      |
| hasCar          |
| hasExplodingCar |
| descending      |
| orderBy         |

| Can be ordered by: |
| ------------------ |
| creationDate       |
| modificationDate   |
| totalPlays         |
| uniquePlays        |
| totalCompletions   |
| uniqueCompletions  |
| likes              |
| queues             |
| fileSize           |
| difficulty         |
| relevance          |
| random             |
| totalDeaths        |
| totalPlayTime      |
| averagePlayTime    |
| totalScreens       |
| totalEntities      |
| bpm                |
| transposeValue     |

### Response: 200

```json
{
  "Levels": [
    {
      "Id": "RTkQa2oA",
      "Name": "YOOOO",
      "Author": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "CreationDate": 1685292695,
      "ModificationDate": 1685292894,
      "TotalPlays": 53,
      "UniquePlays": 5,
      "Likes": 1,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "RTD5U8CW",
      "Name": "Imported Level (RTD5U8CW)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1486151226,
      "ModificationDate": 1486151226,
      "TotalPlays": 2,
      "UniquePlays": 2,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "n8tu7Ix0",
      "Name": "Imported Level (n8tu7Ix0)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1389544809,
      "ModificationDate": 1389544809,
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "eBYzDmmF",
      "Name": "Imported Level (eBYzDmmF)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1462499361,
      "ModificationDate": 1462499361,
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    }
  ],
  "Count": 72937
}
```

## End-point: Get Level with Id

### Method: GET

> ```
> {ip}/api/v1/levels/id/{levelId}
> ```

| Index | Scale      |
| ----- | ---------- |
| 0     | Major      |
| 1     | Pentatonic |
| 2     | Minor      |
| 3     | Chromatic  |

### Response: 200

```json
{
  "Id": "eBYzDmmF",
  "Name": "Imported Level (eBYzDmmF)",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1462499361,
  "ModificationDate": 1462499361,
  "TotalPlays": 0,
  "UniquePlays": 0,
  "TotalCompletions": 0,
  "UniqueCompletions": 0,
  "Likes": 0,
  "Queues": 0,
  "TotalDeaths": 0,
  "TotalPlayTime": 0,
  "Language": 0,
  "Difficulty": 0.0,
  "Analysis": {
    "FileSize": 5913,
    "Bpm": 120,
    "TransposeValue": 0,
    "ScaleIndex": 0,
    "TotalScreens": 32,
    "TotalEntities": 350,
    "HasCar": false,
    "HasExplodingCar": false
  },
  "Albums": [],
  "DailyLevels": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level : User Relation

### Method: GET

> ```
> {ip}/api/v1/levels/id/{levelId}/users/id/{userId}
> ```

### Response: 200

```json
{
  "Completed": true,
  "Liked": false,
  "Queued": false
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level Thumbnail

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/levels/id/{levelId}/thumbnail
> ```

### Response: 200

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Level File

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/levels/id/{levelId}/level
> ```

### Response: 200

```binary
x�ݝ[���

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Like Level

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/like
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unlike Level

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/unLike
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Queue Level

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/queue
> ```

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Unqueue Level

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/unQueue
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Level Metadata

### ⚠️ Moderators (or higher) can do this on any level. People with default permissions can only do this on their own levels.

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/edit
> ```

### Body (**raw**)

```json
{
  "Name": "This is an updated level name"
}
```

### Response: 201

```json
{
  "Id": "eBYzDmmF",
  "Name": "This is an updated level name",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1462499361,
  "ModificationDate": 1462499984,
  "TotalPlays": 0,
  "UniquePlays": 0,
  "TotalCompletions": 0,
  "UniqueCompletions": 0,
  "Likes": 0,
  "Queues": 0,
  "TotalDeaths": 0,
  "TotalPlayTime": 0,
  "Language": 0,
  "Difficulty": 0.0,
  "Analysis": {
    "FileSize": 5913,
    "Bpm": 120,
    "TransposeValue": 0,
    "ScaleIndex": 0,
    "TotalScreens": 32,
    "TotalEntities": 350,
    "HasCar": false,
    "HasExplodingCar": false
  },
  "Albums": [],
  "DailyLevels": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Level

### ⚠️ Moderators (or higher) can do this on any level. People with default permissions can only do this on their own levels.

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/levels/create
> ```

### Body (**raw**)

```json
{
  "Name": "This is a level name!",
  "Language": 0,
  "CreationDate": 1688409183
}
```

### Response: 201

```json
{
  "Id": "7i5Ig3Fj",
  "Name": "This is a level name!",
  "Author": {
    "Id": "f6c4f990-003e-4a9a-8b83-8fee60e9775c",
    "Username": "turecross123",
    "PermissionsType": 0,
    "PublishedLevels": 11,
    "Followers": 0
  },
  "CreationDate": 1688409183,
  "ModificationDate": 1688409183,
  "TotalPlays": 0,
  "UniquePlays": 0,
  "TotalCompletions": 0,
  "UniqueCompletions": 0,
  "Likes": 0,
  "Queues": 0,
  "TotalDeaths": 0,
  "TotalPlayTime": 0,
  "Language": 0,
  "Difficulty": 0,
  "Analysis": {
    "FileSize": 0,
    "Bpm": 0,
    "TransposeValue": 0,
    "ScaleIndex": 0,
    "TotalScreens": 0,
    "TotalEntities": 0,
    "HasCar": false,
    "HasExplodingCar": false
  },
  "Albums": [],
  "DailyLevels": []
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Level File for Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/levels/id/{levelId}/setLevel
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
> {ip}/api/v1/levels/id/{levelId}/setThumbnail
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
> {ip}/api/v1/albums
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
| totalPlays         |
| uniquePlays        |
| levels             |
| fileSize           |
| difficulty         |

### Response: 200

```json
{
  "Albums": [
    {
      "Id": "2c0e429c-4996-44c2-a5b1-b877d44163d9",
      "Author": "DJ Khaled",
      "Name": "We The Best",
      "LinerNotes": "<linerNotes><title>YO MAN</title><header>this header...</header><normal>normal sh*t</normal></linerNotes>",
      "TotalLevels": 2,
      "CreationDate": 1685307965,
      "ModificationDate": 1685307965
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
> {ip}/api/v1/albums/id/{albumId}
> ```

### Response: 200

```json
{
  "Id": "2c0e429c-4996-44c2-a5b1-b877d44163d9",
  "Author": "DJ Khaled",
  "Name": "We The Best",
  "LinerNotes": "<linerNotes><title>YO MAN</title><header>this header...</header><normal>normal sh*t</normal></linerNotes>",
  "TotalLevels": 2,
  "CreationDate": 1685307965,
  "ModificationDate": 1685307965
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Album Thumbnail

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/albums/id/{albumId}/thumbnail
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
> {ip}/api/v1/albums/id/{albumId}/sidePanel
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
> {ip}/api/v1/albums/id/{albumId}/completion
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
> {ip}/api/v1/albums/create
> ```

# Note: LinerNotes should be an XML document.

| LinerNote Element Types |
| ----------------------- |
| title                   |
| header                  |
| normal                  |

### Body (**raw**)

```json
{
  "Name": "We The Best",
  "Author": "DJ Khaled",
  "LinerNotes": "<linerNotes><title>This is a title!</title><header>This is a header!</header><normal>This is normal text</normal></linerNotes>",
  "LevelIds": ["{levelId}", "{otherLevelId}"]
}
```

### Response: 201

```json
{
  "Id": "2c0e429c-4996-44c2-a5b1-b877d44163d9",
  "Author": "DJ Khaled",
  "Name": "We The Best",
  "LinerNotes": "<linerNotes><title>YO MAN</title><header>this header...</header><normal>normal sh*t</normal></linerNotes>",
  "TotalLevels": 2,
  "CreationDate": 1685307965,
  "ModificationDate": 1685307965
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Album

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/albums/id/{albumId}/edit
> ```

# Note: LinerNotes should be an XML document.

| LinerNote Element Types |
| ----------------------- |
| title                   |
| header                  |
| normal                  |

### Body (**raw**)

```json
{
  "Name": "We Not The Best",
  "Author": "DJ Khaled the Second",
  "LinerNotes": "<linerNotes><title>This is still a title!</title><header>This is a header!</header><normal>This is normal text</normal></linerNotes>",
  "LevelIds": ["{levelId}", "{levelId2}"]
}
```

### Response: 201

```json
{
  "Id": "a603d25f-4324-41df-b573-c143679bb7f2",
  "Author": "DJ Khaled the Second",
  "Name": "We Not The Best",
  "LinerNotes": "<linerNotes><title>This is still a title!</title><header>This is a header!</header><normal>This is normal text</normal></linerNotes>",
  "TotalLevels": 2,
  "CreationDate": 1685307965,
  "ModificationDate": 1685307965
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Album Thumbnail

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/albums/id/{albumId}/setThumbnail
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
> {ip}/api/v1/albums/id/{albumId}/setSidePanel
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
> {ip}/api/v1/albums/id/{albumId}/remove
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
> {ip}/api/v1/scores
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
| notes              |
| creationDate       |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "d5ea9148-f1f1-431f-b090-76242be489b5",
      "LevelId": "OkoJyxL9",
      "Position": 0,
      "User": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "Score": 138873478258074,
      "PlayTime": 5709210,
      "Notes": 433,
      "Deaths": 445,
      "Completed": true,
      "CreationDate": 1685923772
    },
    {
      "Id": "2f49f145-6a62-457a-b979-02e34c839228",
      "LevelId": "KIr46vcb",
      "Position": 1,
      "User": {
        "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
        "Username": "Grrheheheha",
        "PermissionsType": 0,
        "PublishedLevels": 1,
        "Followers": 3
      },
      "Score": 140114718107991,
      "PlayTime": 10583,
      "Notes": 144,
      "Deaths": 0,
      "Completed": false,
      "CreationDate": 1685371198
    },
    {
      "Id": "1d12c5f0-8880-4280-9d88-24496f532804",
      "LevelId": "KIr46vcb",
      "Position": 2,
      "User": {
        "Id": "7d6ae6ce-ec54-4efa-a429-3805752fbff7",
        "Username": "CatgirlFishing",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 3
      },
      "Score": 140114718110924,
      "PlayTime": 13516,
      "Notes": 144,
      "Deaths": 0,
      "Completed": true,
      "CreationDate": 1685370065
    },
    {
      "Id": "ff4d868e-93bd-4889-a0d0-b0c1fd3d61cd",
      "LevelId": "KIr46vcb",
      "Position": 3,
      "User": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 3
      },
      "Score": 140114718110941,
      "PlayTime": 13533,
      "Notes": 144,
      "Deaths": 0,
      "Completed": true,
      "CreationDate": 1685369817
    }
  ],
  "Count": 440
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Leaderboard Entry

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}/api/v1/scores/id/{leaderboardEntryId}/remove
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
      "Id": "f0bc1431-2c3e-45ca-92e1-dbb3f4a7c13f",
      "Level": {
        "Id": "fCpVLL1h",
        "Name": "Imported Level (fCpVLL1h)",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevels": 72932,
          "Followers": 1
        },
        "CreationDate": 1364550589,
        "ModificationDate": 1364550589,
        "TotalPlays": 8,
        "UniquePlays": 4,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 6.277778
      },
      "Date": 1685232000,
      "CreationDate": 1685232000,
      "ModificationDate": 1685232000,
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      }
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
> {ip}/api/v1/daily/create
> ```

### Body (**raw**)

```json
{
  "LevelId": "{levelId}",
  "Date": 1685232000
}
```

### Response: 201

```json
{
  "Id": "f0bc1431-2c3e-45ca-92e1-dbb3f4a7c13f",
  "Level": {
    "Id": "fCpVLL1h",
    "Name": "Imported Level (fCpVLL1h)",
    "Author": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevels": 72932,
      "Followers": 1
    },
    "CreationDate": 1364550589,
    "ModificationDate": 1364550589,
    "TotalPlays": 8,
    "UniquePlays": 4,
    "Likes": 0,
    "Queues": 0,
    "Difficulty": 6.277778
  },
  "Date": 1685232000,
  "CreationDate": 1685232000,
  "ModificationDate": 1685232000,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  }
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Daily Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/daily/{id}/edit
> ```

### Body (**raw**)

```json
{
  "LevelId": "{levelId}",
  "Date": 1688574783
}
```

### Response: 201

```json
{
  "Id": "f0bc1431-2c3e-45ca-92e1-dbb3f4a7c13f",
  "Level": {
    "Id": "fCpVLL1h",
    "Name": "Imported Level (fCpVLL1h)",
    "Author": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevels": 72932,
      "Followers": 1
    },
    "CreationDate": 1364550589,
    "ModificationDate": 1364550589,
    "TotalPlays": 8,
    "UniquePlays": 4,
    "Likes": 0,
    "Queues": 0,
    "Difficulty": 6.277778
  },
  "Date": 1688574783,
  "CreationDate": 1685232000,
  "ModificationDate": 1688574783,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  }
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Daily Level

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/daily/id/{dailyLevelId}/remove
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
> {ip}/api/v1/news
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
      "Id": "19d5cc55-0f96-4ddc-897b-12a9d0c69a2d",
      "CreationDate": 1685309490,
      "ModificationDate": 1685309490,
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "Language": "global",
      "Title": "DJ Khaled is stuck in a tree!",
      "Summary": "This ain't no joke team!",
      "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
      "Url": "http://djkhaled.com"
    }
  ],
  "Count": 1
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get News Entry With Id

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/news/id/{newsEntryId}
> ```

### Response: 200

```json
{
  "Id": "19d5cc55-0f96-4ddc-897b-12a9d0c69a2d",
  "CreationDate": 1685309490,
  "ModificationDate": 1685309490,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "Language": "global",
  "Title": "DJ Khaled is stuck in a tree!",
  "Summary": "This ain't no joke team!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
  "Url": "http://djkhaled.com"
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create News Entry

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/news/create
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
  "Id": "19d5cc55-0f96-4ddc-897b-12a9d0c69a2d",
  "CreationDate": 1685309490,
  "ModificationDate": 1685309490,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
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
> {ip}/api/v1/news/id/{newsEntryId}/edit
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
  "Id": "19d5cc55-0f96-4ddc-897b-12a9d0c69a2d",
  "CreationDate": 1685309490,
  "ModificationDate": 1688417626,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
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
> {ip}/api/v1/news/id/{newsEntryId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set News Entry Thumbnail

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/news/id/{newsEntryId}/setThumbnail
> ```

### Body

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get News Entry Thumbnail

### Method: GET

> ```
> {ip}/api/v1/news/id/{newsEntryId}/thumbnail
> ```

### Response: 201

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Events (Recent Activity) {#Events}

## End-point: Get Events

### 🔑 This does not require the `Authorization` header.

### Method: GET

> ```
> {ip}/api/v1/events
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
| 2     | Queue                |
| 3     | Follow               |
| 4     | Score Submission     |
| 5     | Account Registration |

| Can be ordered by: |
| ------------------ |
| date               |

### Response: 200

```json
{
  "Events": [
    {
      "Id": "48c47484-db2b-4290-ac28-bd8676a1d777",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "ContentLeaderboardEntry": {
        "Id": "6fa98c3e-ae9a-40d6-86ac-affda2b9f12c",
        "LevelId": "LCpuvQHN",
        "Position": -1,
        "User": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 4,
          "Followers": 3
        },
        "Score": 140733193392665,
        "PlayTime": 4633,
        "Notes": 0,
        "Deaths": 0,
        "Completed": true,
        "CreationDate": 1686227241
      },
      "CreationDate": 1686227241
    },
    {
      "Id": "6b447ee4-aca0-4a00-8697-02a74cb6f856",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "ContentLeaderboardEntry": {
        "Id": "0373d633-46f1-4aac-a783-a10b1f7e4e59",
        "LevelId": "LCpuvQHN",
        "Position": -1,
        "User": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 4,
          "Followers": 3
        },
        "Score": 140733193392665,
        "PlayTime": 4633,
        "Notes": 0,
        "Deaths": 0,
        "Completed": true,
        "CreationDate": 1686227038
      },
      "CreationDate": 1686227038
    },
    {
      "Id": "1867d88a-6f83-4f53-8edb-9033d77cca7e",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "ContentLeaderboardEntry": {
        "Id": "f659aa9f-4b7c-414e-817d-3521af33a2cd",
        "LevelId": "LCpuvQHN",
        "Position": -1,
        "User": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 4,
          "Followers": 3
        },
        "Score": 140733193392665,
        "PlayTime": 4633,
        "Notes": 0,
        "Deaths": 0,
        "Completed": true,
        "CreationDate": 1686226904
      },
      "CreationDate": 1686226904
    },
    {
      "Id": "a5dc14f1-3893-4d74-9b6f-468aca324c13",
      "EventType": 0,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "ContentLevel": {
        "Id": "LCpuvQHN",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 4,
          "Followers": 3
        },
        "CreationDate": 1686226773,
        "ModificationDate": 1686227241,
        "TotalPlays": 6,
        "UniquePlays": 1,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "CreationDate": 1686226773
    }
  ],
  "Count": 477
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Event

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}/api/v1/events/id/{eventId}/remove
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
> {ip}/api/v1/reports
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
      "Id": "012a92f9-4364-4ef7-8fa6-6e1d4b5fef2f",
      "ContentLevel": {
        "Id": "KIr46vcb",
        "Name": "ow",
        "Author": {
          "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
          "Username": "Grrheheheha",
          "PermissionsType": 0,
          "PublishedLevels": 1,
          "Followers": 3
        },
        "CreationDate": 1685311293,
        "ModificationDate": 1685311293,
        "TotalPlays": 47,
        "UniquePlays": 4,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "ContentType": 1,
      "ReasonType": 0,
      "CreationDate": 1685312908,
      "Author": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 3
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
> {ip}/api/v1/reports/id/{reportId}
> ```

### Response: 200

```json
{
  "Id": "012a92f9-4364-4ef7-8fa6-6e1d4b5fef2f",
  "ContentLevel": {
    "Id": "KIr46vcb",
    "Name": "ow",
    "Author": {
      "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
      "Username": "Grrheheheha",
      "PermissionsType": 0,
      "PublishedLevels": 1,
      "Followers": 3
    },
    "CreationDate": 1685311293,
    "ModificationDate": 1685311293,
    "TotalPlays": 47,
    "UniquePlays": 4,
    "Likes": 0,
    "Queues": 0,
    "Difficulty": 0.0
  },
  "ContentType": 1,
  "ReasonType": 0,
  "CreationDate": 1685312908,
  "Author": {
    "Id": "df197af6-e901-465a-bd03-cef2b851b368",
    "Username": "Jazzkha11",
    "PermissionsType": 0,
    "PublishedLevels": 0,
    "Followers": 3
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
> {ip}/api/v1/reports/create
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
> {ip}/api/v1/reports/id/{reportId}/remove
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
> {ip}/api/v1/punishments
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
      "Id": "530f2529-b125-4b44-94e5-7d81cd3e39b7",
      "Recipient": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 4,
        "Followers": 3
      },
      "PunishmentType": 0,
      "Reason": "GET YALLS BATT OUT OF HERE... PLEASE!",
      "Revoked": true,
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1686062438,
      "ModificationDate": 1686062438,
      "ExpiryDate": 1686175200,
      "RevokeDate": 1686063520
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
> {ip}/api/v1/punishments/create
> ```

| PunishmentType | Punishment |
| -------------- | ---------- |
| 0              | Ban        |

### Body (**raw**)

```json
{
  "UserId": "{userId}",
  "PunishmentType": 0,
  "Reason": "GET YALLS BATT OUT OF HERE...",
  "ExpiryDate": 1696523583
}
```

### Response: 201

```json
{
  "Id": "530f2529-b125-4b44-94e5-7d81cd3e39b7",
  "Recipient": {
    "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
    "Username": "turecross321",
    "PermissionsType": 0,
    "PublishedLevels": 2,
    "Followers": 3
  },
  "PunishmentType": 0,
  "Reason": "GET YALLS BATT OUT OF HERE...",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1686062438,
  "ModificationDate": 1686062438,
  "ExpiryDate": 1696523583,
  "RevokeDate": null
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}/api/v1/punishments/id/{punishmentId}/edit
> ```

| PunishmentType | Punishment |
| -------------- | ---------- |
| 0              | Ban        |

### Body (**raw**)

```json
{
  "UserId": "c479afbc-84da-4b74-8077-012e3eed7aec",
  "PunishmentType": 0,
  "Reason": "GET YALLS BATT OUT OF HERE... PLEASE!",
  "ExpiryDate": 1796491983
}
```

### Response: 201

```json
{
  "Id": "530f2529-b125-4b44-94e5-7d81cd3e39b7",
  "Recipient": {
    "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
    "Username": "turecross321",
    "PermissionsType": 0,
    "PublishedLevels": 2,
    "Followers": 3
  },
  "PunishmentType": 0,
  "Reason": "GET YALLS BATT OUT OF HERE... PLEASE",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1686062438,
  "ModificationDate": 1686062673,
  "ExpiryDate": 1696523583,
  "RevokeDate": null
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Revoke Punishment

### ⚠️ This requires moderator (or higher) permissions.

### Method: POST

> ```
> {ip}/api/v1/punishments/id/{punishmentId}/revoke
> ```

### Response: 200

```json
{
  "Id": "530f2529-b125-4b44-94e5-7d81cd3e39b7",
  "Recipient": {
    "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
    "Username": "turecross321",
    "PermissionsType": 0,
    "PublishedLevels": 2,
    "Followers": 3
  },
  "PunishmentType": 0,
  "Reason": "GET YALLS BATT OUT OF HERE... PLEASE",
  "Revoked": true,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1686062438,
  "ModificationDate": 1686062673,
  "ExpiryDate": 1696523583,
  "RevokeDate": 1688418080
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

# 📁 Community Tabs {#CommunityTabs}

## End-point: Get Community Tabs

### Method: GET

> ```
> {ip}/api/v1/communityTabs
> ```

| Value | Content Type |
| ----- | ------------ |
| 0     | Users        |
| 1     | Levels       |
| 2     | Events       |

### Response: 200

```json
{
  "CommunityTabs": [
    {
      "Id": "0c208c04-33d4-4703-a878-6d6aed107f1a",
      "ContentType": 0,
      "Title": "Users",
      "Description": "Users!",
      "ButtonLabel": "Users",
      "Query": "orderBy=creationDate",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1685307608,
      "ModificationDate": 1686090259
    },
    {
      "Id": "8614e7ba-f3c8-4794-89e0-b583f2924373",
      "ContentType": 1,
      "Title": "Level Example",
      "Description": "Levels!",
      "ButtonLabel": "Level Example",
      "Query": "orderBy=fileSize",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1685307755,
      "ModificationDate": 1685307755
    },
    {
      "Id": "e7f24386-4b12-4a33-b9c2-10237a329f92",
      "ContentType": 2,
      "Title": "Event Example",
      "Description": "Events!",
      "ButtonLabel": "Event Example",
      "Query": "eventTypes=0",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1685315504,
      "ModificationDate": 1685315504
    },
    {
      "Id": "54e0bcc3-9e30-420a-b9a0-90b1d74f39d2",
      "ContentType": 1,
      "Title": "Wacha Wam BAM KACHAM",
      "Description": "This Sh*t is what they're all talking about. Check this out...",
      "ButtonLabel": "VERY GOOD SHIT",
      "Query": "orderBy=totalScreens&hasExplodingCar=true",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": 1686063152,
      "ModificationDate": 1686063201
    }
  ],
  "Count": 4
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Create Community Tab

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/communityTabs/create
> ```

| Value | Content Type |
| ----- | ------------ |
| 0     | Users        |
| 1     | Levels       |
| 2     | Events       |

## Note: Both API and game query works, but it's recomended that you use API query.

### Body (**raw**)

```json
{
  "ContentType": 1,
  "ButtonLabel": "GOOD SHIT",
  "Title": "Wacha Wam",
  "Description": "This Sh*t is what it's all about. Check this out...",
  "Query": "orderBy=totalScreens&hasExplodingCar=true"
}
```

### Response: 201

```json
{
  "Id": "54e0bcc3-9e30-420a-b9a0-90b1d74f39d2",
  "ContentType": 1,
  "Title": "Wacha Wam",
  "Description": "This Sh*t is what it's all about. Check this out...",
  "ButtonLabel": "GOOD SHIT",
  "Query": "orderBy=totalScreens&hasExplodingCar=true",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1686063152,
  "ModificationDate": 1686063201
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Edit Community Tab

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/communityTabs/id/{communityTabId}/edit
> ```

### Body (**raw**)

```json
{
  "ContentType": 1,
  "ButtonLabel": "VERY GOOD SHIT",
  "Title": "Wacha Wam BAM KACHAM",
  "Description": "This Sh*t is what they're all talking about. Check this out...",
  "Query": "orderBy=totalScreens&hasExplodingCar=true"
}
```

### Response: 201

```json
{
  "Id": "54e0bcc3-9e30-420a-b9a0-90b1d74f39d2",
  "ContentType": 1,
  "Title": "Wacha Wam BAM KACHAM",
  "Description": "This Sh*t is what they're all talking about. Check this out...",
  "ButtonLabel": "VERY GOOD SHIT",
  "Query": "orderBy=totalScreens&hasExplodingCar=true",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": 1686063152,
  "ModificationDate": 1687042221
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Set Thumbnail for Community Tab

### ⚠️ This requires administrator permissions.

### Method: POST

> ```
> {ip}/api/v1/communityTabs/id/{communityTabId}/setThumbnail
> ```

### Body (**binary**)

Should be a PNG

### Response: 201

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Remove Community Tab

### Method: POST

> ```
> {ip}/api/v1/communityTabs/{communityTabId}/remove
> ```

### Response: 200

```json

```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Community Tab Thumbnail

### Method: GET

> ```
> {ip}/api/v1/news/id/{newsEntryId}/thumbnail
> ```

### Response: 201

```binary
�PNG

���
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃
