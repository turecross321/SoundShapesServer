# API Documentation

## Information

Every endpoint, unless specified otherwise, requires an `Authorization` header.
The value of `Authorization` should always be your **Session Id**, unless specified otherwise. Your Session Id is the Id which is provided when logging in.

Date is always UTC, and is always formatted like `YYYY-MM-DD`.

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
  "Id": "4aaa94e7-0728-44b7-8bc8-d534a7047c9d",
  "ExpiryDate": "2023-06-07T13:52:14.8652596+00:00",
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
      "IpAddress": "192.168.1.134",
      "Authorized": false,
      "OneTimeUse": false,
      "CreationDate": "2023-06-03T11:02:31.758+00:00",
      "ModificationDate": "2023-06-03T11:02:31.758+00:00"
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
      "Id": "df197af6-e901-465a-bd03-cef2b851b368",
      "Username": "Jazzkha11",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 0
    },
    {
      "Id": "7d6ae6ce-ec54-4efa-a429-3805752fbff7",
      "Username": "CatgirlFishing",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 0
    },
    {
      "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
      "Username": "Grrheheheha",
      "PermissionsType": 0,
      "PublishedLevels": 0,
      "Followers": 0
    },
    {
      "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
      "Username": "turecross321",
      "PermissionsType": 0,
      "PublishedLevels": 3,
      "Followers": 1
    },
    {
      "Id": "00000000-0000-0000-0000-000000000000",
      "Username": "admin",
      "PermissionsType": 2,
      "PublishedLevels": 72932,
      "Followers": 0
    }
  ],
  "Count": 5
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
  "Id": "d95980b4-ba6b-4c68-93ca-1a2c710ef419",
  "Username": "jvyden420",
  "PermissionsType": 0,
  "CreationDate": "2023-05-29T23:37:42.4915544+00:00",
  "LastGameLogin": "2023-05-29T23:38:22.6324251+00:00",
  "LastEventDate": "2023-05-29T23:50:01.4971192+00:00",
  "Followers": 0,
  "Following": 0,
  "LikedLevels": 0,
  "QueuedLevels": 0,
  "PublishedLevels": 0,
  "Events": 9,
  "PlayedLevels": 2,
  "TotalDeaths": 8,
  "TotalPlayTime": 132128
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
  "Id": "d95980b4-ba6b-4c68-93ca-1a2c710ef419",
  "Username": "jvyden420",
  "PermissionsType": 0,
  "CreationDate": "2023-05-29T23:37:42.4915544+00:00",
  "LastGameLogin": "2023-05-29T23:38:22.6324251+00:00",
  "LastEventDate": "2023-05-29T23:50:01.4971192+00:00",
  "Followers": 0,
  "Following": 0,
  "LikedLevels": 0,
  "QueuedLevels": 0,
  "PublishedLevels": 0,
  "Events": 9,
  "PlayedLevels": 2,
  "TotalDeaths": 8,
  "TotalPlayTime": 132128
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
      "Name": "1",
      "Author": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
      "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
      "TotalPlays": 29,
      "UniquePlays": 4,
      "Likes": 0,
      "Queues": 1,
      "Difficulty": 0.0
    },
    {
      "Id": "RTD5U8CW",
      "Name": "2",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2017-02-03T19:47:06+00:00",
      "ModificationDate": "2017-02-03T19:47:06+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 1,
      "Difficulty": 0.0
    },
    {
      "Id": "n8tu7Ix0",
      "Name": "3",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2014-01-12T16:40:09+00:00",
      "ModificationDate": "2014-01-12T16:40:09+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 1,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "eBYzDmmF",
      "Name": "4",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2016-05-06T01:49:21+00:00",
      "ModificationDate": "2016-05-06T01:49:21+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 1,
      "Difficulty": 0.0
    },
    {
      "Id": "judLGnNp",
      "Name": "Imported Level (judLGnNp)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2015-08-28T17:30:03+00:00",
      "ModificationDate": "2015-08-28T17:30:03+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "cABNDlyS",
      "Name": "Imported Level (cABNDlyS)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2014-08-10T04:22:50+00:00",
      "ModificationDate": "2014-08-10T04:22:50+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "0exJ7YQ1",
      "Name": "Imported Level (0exJ7YQ1)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2016-06-13T11:19:47+00:00",
      "ModificationDate": "2016-06-13T11:19:47+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "zmHUNXMg",
      "Name": "Imported Level (zmHUNXMg)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2015-09-05T06:40:52+00:00",
      "ModificationDate": "2015-09-05T06:40:52+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    },
    {
      "Id": "M0VLC7vu",
      "Name": "Imported Level (M0VLC7vu)",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "CreationDate": "2015-10-13T18:56:47+00:00",
      "ModificationDate": "2015-10-13T18:56:47+00:00",
      "TotalPlays": 0,
      "UniquePlays": 0,
      "Likes": 0,
      "Queues": 0,
      "Difficulty": 0.0
    }
  ],
  "Count": 72935
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
  "Id": "RTkQa2oA",
  "Name": "1",
  "Author": {
    "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
    "Username": "turecross321",
    "PermissionsType": 0,
    "PublishedLevels": 3,
    "Followers": 1
  },
  "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
  "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
  "TotalPlays": 29,
  "UniquePlays": 4,
  "TotalCompletions": 27,
  "UniqueCompletions": 4,
  "Likes": 0,
  "Queues": 1,
  "TotalDeaths": 0,
  "TotalPlayTime": 169604,
  "Language": 0,
  "Difficulty": 0.0,
  "Analysis": {
    "FileSize": 1130,
    "Bpm": 120,
    "TransposeValue": 0,
    "ScaleIndex": 0,
    "TotalScreens": 30,
    "TotalEntities": 38,
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
  "Id": "RTkQa2oA",
  "Name": "This is an updated level name",
  "Author": {
    "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
    "Username": "turecross321",
    "PermissionsType": 0,
    "PublishedLevels": 3,
    "Followers": 1
  },
  "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
  "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
  "TotalPlays": 29,
  "UniquePlays": 4,
  "TotalCompletions": 27,
  "UniqueCompletions": 4,
  "Likes": 0,
  "Queues": 1,
  "TotalDeaths": 0,
  "TotalPlayTime": 169604,
  "Language": 4,
  "Difficulty": 0.0,
  "Analysis": {
    "FileSize": 1130,
    "Bpm": 120,
    "TransposeValue": 0,
    "ScaleIndex": 0,
    "TotalScreens": 30,
    "TotalEntities": 38,
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
  "ModificationDate": "2023-05-16T09:58:43.5485486+00:00"
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
  "CreationDate": "2023-05-16T09:58:43.5485486+00:00",
  "ModificationDate": "2023-05-16T09:58:43.5485486+00:00",
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
> {ip}/api/v1/albums/id/{albumId}
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
> {ip}/api/v1/albums/id/{albumId}/edit
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
| date               |

### Response: 200

```json
{
  "Entries": [
    {
      "Id": "a311ef0c-222d-419e-8c75-aa1b8ec54762",
      "LevelId": "j7n0BF2U",
      "Position": 0,
      "User": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "Score": 140000000006766,
      "PlayTime": 5883,
      "Notes": 8,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-29T21:59:10.4676202+00:00"
    },
    {
      "Id": "026a1711-e332-47af-814c-8d5e81d1d893",
      "LevelId": "carDLC_metal",
      "Position": 1,
      "User": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140209207743338,
      "PlayTime": 0,
      "Notes": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-28T21:36:25.4930248+00:00"
    },
    {
      "Id": "2a26384e-0b46-42c8-b78b-2e13dcfba994",
      "LevelId": "a47a6WAO",
      "Position": 2,
      "User": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140338056662214,
      "PlayTime": 265414,
      "Notes": 92,
      "Deaths": 1,
      "Completed": true,
      "Date": "2023-05-28T22:00:52.491621+00:00"
    },
    {
      "Id": "9a07000e-4f9a-4054-aa24-44844d5f69ff",
      "LevelId": "a47a6WAO",
      "Position": 3,
      "User": {
        "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
        "Username": "Grrheheheha",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140338056711200,
      "PlayTime": 314400,
      "Notes": 92,
      "Deaths": 1,
      "Completed": true,
      "Date": "2023-05-28T21:48:15.5933151+00:00"
    },
    {
      "Id": "743f792e-444f-44d1-a921-c47196bb8566",
      "LevelId": "beckCities",
      "Position": 4,
      "User": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140338056746627,
      "PlayTime": 0,
      "Notes": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-28T21:36:26.2675246+00:00"
    },
    {
      "Id": "93a8ef66-a02a-4ef1-baad-df2be00f68a7",
      "LevelId": "beckCities",
      "Position": 5,
      "User": {
        "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
        "Username": "Grrheheheha",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140338056748709,
      "PlayTime": 0,
      "Notes": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-28T21:07:02.7669136+00:00"
    },
    {
      "Id": "3369aa68-3ddc-42e5-94bd-90b90ee273df",
      "LevelId": "pixeljam3",
      "Position": 6,
      "User": {
        "Id": "df197af6-e901-465a-bd03-cef2b851b368",
        "Username": "Jazzkha11",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140359531722703,
      "PlayTime": 0,
      "Notes": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-28T21:36:27.0870515+00:00"
    },
    {
      "Id": "57f97a76-30bb-4f13-94d2-8ff5f61ec39a",
      "LevelId": "pixeljam3",
      "Position": 7,
      "User": {
        "Id": "16f2a7c8-9bc8-4f7b-bc16-c6718f8e525f",
        "Username": "Grrheheheha",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140359531728000,
      "PlayTime": 0,
      "Notes": 0,
      "Deaths": 0,
      "Completed": true,
      "Date": "2023-05-28T21:07:02.8772648+00:00"
    },
    {
      "Id": "436cf253-e214-4336-892d-4edc2e19fda0",
      "LevelId": "fCpVLL1h",
      "Position": 8,
      "User": {
        "Id": "7d6ae6ce-ec54-4efa-a429-3805752fbff7",
        "Username": "CatgirlFishing",
        "PermissionsType": 0,
        "PublishedLevels": 0,
        "Followers": 0
      },
      "Score": 140385301447329,
      "PlayTime": 410273,
      "Notes": 81,
      "Deaths": 24,
      "Completed": true,
      "Date": "2023-05-28T21:46:47.5231113+00:00"
    }
  ],
  "Count": 82
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
          "Followers": 0
        },
        "CreationDate": "2013-03-29T09:49:49+00:00",
        "ModificationDate": "2013-03-29T09:49:49+00:00",
        "TotalPlays": 2,
        "UniquePlays": 2,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 6.0
      },
      "Date": "2023-05-28T00:00:00+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
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
  "Date": "2023-06-03"
}
```

### Response: 201

```json
{
  "Id": "a5879527-83d0-41c7-abab-0fc750672012",
  "Level": {
    "Id": "RTkQa2oA",
    "Name": "1",
    "Author": {
      "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
      "Username": "turecross321",
      "PermissionsType": 0,
      "PublishedLevels": 3,
      "Followers": 1
    },
    "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
    "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
    "TotalPlays": 29,
    "UniquePlays": 4,
    "Likes": 0,
    "Queues": 1,
    "Difficulty": 0.0
  },
  "Date": "2023-06-02T22:00:00+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 0
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
      "CreationDate": "2023-05-28T21:31:30.8071334+00:00",
      "ModificationDate": "2023-05-28T21:31:30.8072189+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
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
  "CreationDate": "2023-05-28T21:31:30.8071334+00:00",
  "ModificationDate": "2023-05-28T21:31:30.8072189+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 0
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
  "CreationDate": "2023-05-28T21:31:30.8071334+00:00",
  "ModificationDate": "2023-05-28T21:31:30.8071334+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 0
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
  "CreationDate": "2023-05-28T21:31:30.8071334+00:00",
  "ModificationDate": "2023-05-29T21:31:30.8072189+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 0
  },
  "Language": "global",
  "Title": "DJ Khaled is stuck in a tree!",
  "Summary": "This ain't no joke team!",
  "FullText": "DJ Khaled has had jet ski trouble in the past, getting lost after dark on his way home from Rick Ross' house, and it seems he is currently running into some more trouble on the waterways. The producer has taken to Instagram to share his latest at-sea experience, documenting a trip to Diddy's house to hang with Drake via a \"secret route.\" His special detour turned out to be blocked by fallen trees, however, and when Khaled attempted to take his jet ski over the downed branches he got stuck, then cut his hand and leg trying to untangle the vehicle. He remains positive, however, repeating that the key to overcoming life's literal and metaphorical roadblocks is to not panic.",
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
      "Id": "10b1d061-9b92-420b-93cd-79e10581e293",
      "EventType": 1,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "ContentLevel": {
        "Id": "j7n0BF2U",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-05-29T21:51:07.5685175+00:00",
        "ModificationDate": "2023-05-29T21:51:07.5685175+00:00",
        "TotalPlays": 5,
        "UniquePlays": 1,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-06-03T11:41:52.5210808+00:00"
    },
    {
      "Id": "833284ba-6490-4b8e-924f-2e19a22d9dd2",
      "EventType": 1,
      "Actor": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
      },
      "ContentLevel": {
        "Id": "qnJoXGCr",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-06-01T20:40:10.2468416+00:00",
        "ModificationDate": "2023-06-02T14:29:05.0087392+00:00",
        "TotalPlays": 3,
        "UniquePlays": 1,
        "Likes": 1,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-06-03T11:35:32.4623161+00:00"
    },
    {
      "Id": "6669f782-7ce6-4845-ade9-96bc582815a4",
      "EventType": 1,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "ContentLevel": {
        "Id": "qnJoXGCr",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-06-01T20:40:10.2468416+00:00",
        "ModificationDate": "2023-06-02T14:29:05.0087392+00:00",
        "TotalPlays": 3,
        "UniquePlays": 1,
        "Likes": 1,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-06-01T22:17:59.5955077+00:00"
    },
    {
      "Id": "3a0677c7-46c7-4a4d-bade-ae905afadd61",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "ContentLeaderboardEntry": {
        "Id": "ad602b5a-9cbc-4a14-b83b-2b54608085ee",
        "LevelId": "qnJoXGCr",
        "Position": 0,
        "User": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "Score": 140728898420736,
        "PlayTime": 6766,
        "Notes": 1,
        "Deaths": 0,
        "Completed": true,
        "Date": "2023-06-01T20:54:05.9290033+00:00"
      },
      "Date": "2023-06-01T20:54:05.9323973+00:00"
    },
    {
      "Id": "edf6f6f3-fca7-4b29-941e-056c70ccf838",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "Date": "2023-06-01T20:43:05.8663387+00:00"
    },
    {
      "Id": "b2bcdd53-eff4-4b16-8280-1148b0186ef1",
      "EventType": 4,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "Date": "2023-06-01T20:40:10.8962975+00:00"
    },
    {
      "Id": "b90fd008-3121-4022-9f90-bcdfe1de5468",
      "EventType": 0,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "ContentLevel": {
        "Id": "qnJoXGCr",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-06-01T20:40:10.2468416+00:00",
        "ModificationDate": "2023-06-02T14:29:05.0087392+00:00",
        "TotalPlays": 3,
        "UniquePlays": 1,
        "Likes": 1,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-06-01T20:40:10.2613119+00:00"
    },
    {
      "Id": "8289861b-0557-42b1-b7a3-8a61bd67d62c",
      "EventType": 2,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "ContentLevel": {
        "Id": "j7n0BF2U",
        "Name": "Untitled",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-05-29T21:51:07.5685175+00:00",
        "ModificationDate": "2023-05-29T21:51:07.5685175+00:00",
        "TotalPlays": 5,
        "UniquePlays": 1,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-05-30T18:13:56.0971931+00:00"
    },
    {
      "Id": "cfd3071b-a0be-46e6-96ec-9613dbf3fa7f",
      "EventType": 1,
      "Actor": {
        "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
        "Username": "turecross321",
        "PermissionsType": 0,
        "PublishedLevels": 3,
        "Followers": 1
      },
      "ContentLevel": {
        "Id": "0nKxEfms",
        "Name": "Imported Level (0nKxEfms)",
        "Author": {
          "Id": "00000000-0000-0000-0000-000000000000",
          "Username": "admin",
          "PermissionsType": 2,
          "PublishedLevels": 72932,
          "Followers": 0
        },
        "CreationDate": "2019-01-29T22:23:10+00:00",
        "ModificationDate": "2019-01-29T22:23:10+00:00",
        "TotalPlays": 0,
        "UniquePlays": 0,
        "Likes": 0,
        "Queues": 0,
        "Difficulty": 0.0
      },
      "Date": "2023-05-30T18:12:28.3417066+00:00"
    }
  ],
  "Count": 109
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
      "Id": "c3c32b17-3eaf-4ea8-9827-c053b6da2dca",
      "ContentLevel": {
        "Id": "RTkQa2oA",
        "Name": "1",
        "Author": {
          "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
          "Username": "turecross321",
          "PermissionsType": 0,
          "PublishedLevels": 3,
          "Followers": 1
        },
        "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
        "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
        "TotalPlays": 29,
        "UniquePlays": 4,
        "Likes": 0,
        "Queues": 1,
        "Difficulty": 0.0
      },
      "ContentType": 1,
      "ReasonType": 1,
      "Date": "2023-06-03T13:05:58.8086011+00:00",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 0
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
  "Id": "c3c32b17-3eaf-4ea8-9827-c053b6da2dca",
  "ContentLevel": {
    "Id": "RTkQa2oA",
    "Name": "1",
    "Author": {
      "Id": "c479afbc-84da-4b74-8077-012e3eed7aec",
      "Username": "turecross321",
      "PermissionsType": 0,
      "PublishedLevels": 3,
      "Followers": 1
    },
    "CreationDate": "2023-05-28T16:51:35.6601969+00:00",
    "ModificationDate": "2023-05-28T16:54:54.604908+00:00",
    "TotalPlays": 29,
    "UniquePlays": 4,
    "Likes": 0,
    "Queues": 1,
    "Difficulty": 0.0
  },
  "ContentType": 1,
  "ReasonType": 1,
  "Date": "2023-06-03T13:05:58.8086011+00:00",
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 0
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
      "CreationDate": "2023-06-06T14:40:38.6286938+00:00",
      "ExpiryDate": "2023-06-07T00:00:00+00:00",
      "RevokeDate": null
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
  "ExpiryDate": "2023-06-07"
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
  "CreationDate": "2023-06-06T14:40:38.6286938+00:00",
  "ExpiryDate": "2023-06-07T00:00:00+00:00",
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
  "ExpiryDate": "2023-06-08"
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
  "Reason": "GET YALLS BATT OUT OF HERE... PLEASE!",
  "Revoked": false,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": "2023-06-06T14:40:38.6286938+00:00",
  "ExpiryDate": "2023-06-07T22:00:00+00:00",
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
  "Reason": "GET YALLS BATT OUT OF HERE... PLEASE!",
  "Revoked": true,
  "Author": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "Username": "admin",
    "PermissionsType": 2,
    "PublishedLevels": 72932,
    "Followers": 1
  },
  "CreationDate": "2023-06-06T14:40:38.6286938+00:00",
  "ExpiryDate": "2023-06-07T22:00:00+00:00",
  "RevokeDate": "2023-06-06T14:58:40.0585106+00:00"
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
      "Title": "User Example",
      "Description": "Users!",
      "ButtonLabel": "User Example",
      "Query": "orderBy=lastGameLogin",
      "Author": {
        "Id": "00000000-0000-0000-0000-000000000000",
        "Username": "admin",
        "PermissionsType": 2,
        "PublishedLevels": 72932,
        "Followers": 1
      },
      "CreationDate": "2023-05-28T21:00:08.9981183+00:00",
      "ModificationDate": "2023-05-28T21:00:08.9981833+00:00"
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
      "CreationDate": "2023-05-28T21:02:35.8707509+00:00",
      "ModificationDate": "2023-05-28T21:02:35.8707512+00:00"
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
      "CreationDate": "2023-05-28T23:11:44.7594895+00:00",
      "ModificationDate": "2023-05-28T23:11:44.7595407+00:00"
    }
  ],
  "Count": 3
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: Get Community Tab With Id

### Method: GET

> ```
> {ip}/api/v1/communityTabs/id/{communityTabId}
> ```

| Value | Content Type |
| ----- | ------------ |
| 0     | Users        |
| 1     | Levels       |
| 2     | Events       |

### Response: 200

```json
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
  "CreationDate": "2023-05-28T23:11:44.7594895+00:00",
  "ModificationDate": "2023-05-28T23:11:44.7595407+00:00"
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
  "CreationDate": "2023-06-06T14:52:32.9890583+00:00",
  "ModificationDate": "2023-06-06T14:52:32.989151+00:00"
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
  "CreationDate": "2023-06-06T14:52:32.9890583+00:00",
  "ModificationDate": "2023-06-06T14:53:21.1224357+00:00"
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
