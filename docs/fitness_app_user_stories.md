# Elevate Fitness App - AI Implementation Specification

This markdown is a structured transcription of the PDF so an AI coding agent can use it directly.

## Goal
Implement backend services for a fitness application using the user stories, acceptance criteria, endpoints, business rules, database schemas, events, and service interactions below.

---

# Epic 1: Authentication & Identity Service
Endpoints:
- POST /api/v1/auth/register
- POST /api/v1/auth/complete-profile
- POST /api/v1/auth/login
- POST /api/v1/auth/forgot-password
- POST /api/v1/auth/verify-otp
- POST /api/v1/auth/reset-password
- POST /api/v1/auth/refresh-token
- POST /api/v1/auth/logout

Rules:
- Password minimum 6 chars, 1 uppercase, 1 number.
- Never store raw passwords.
- Use bcrypt hash.
- Lockout after 5 failed logins in 15 minutes.
- OTP = 6 digits, hashed, expires after 10 minutes.
- OTP resend cooldown = 30 seconds.
- Refresh tokens rotate on every use.

---

# Epic 2: User Profile & Settings Service

Endpoints:
- GET/PUT /profile
- POST /profile/picture
- PUT /profile/change-password
- GET/PUT /settings

Rules:
- Profile picture: JPG/PNG only.
- Max upload size: 5MB.
- Settings update behaves like PATCH (partial update).
- IsPremiumCached is synced from subscription events.

---

# Epic 3: Fitness Calculation Engine (FCE)

Endpoints:
- POST /fitness/weight-goal-activity
- POST /fitness/calculate
- POST /fitness/assign-plan
- GET /fitness/metrics/{userId}
- GET /fitness/stats/{userId}
- PUT /fitness/recalculate/{userId}
- GET /fitness/plan-configs
- GET /fitness/plans/{planId}

Enums:
Goal:
- Lose Weight
- Get Fitter
- Gain Weight
- Gain More Flexible
- Learn the Basic

Activity Level:
- Rookie
- Beginner
- Intermediate
- Advance
- TrueBeast

BMR:
Male:
10×weight + 6.25×height − 5×age + 5

Female:
10×weight + 6.25×height − 5×age − 161

Activity Factors:
- Rookie = 1.2
- Beginner = 1.375
- Intermediate = 1.55
- Advance = 1.725
- TrueBeast = 1.9

TDEE:
BMR × ActivityFactor

Calorie Target:
- Lose Weight = TDEE - 500
- Gain Weight = TDEE + 300
- Gain More Flexible = TDEE + 150
- Get Fitter = TDEE
- Learn the Basic = TDEE

Status:
- <=1800 => Weak
- 1801-2500 => Normal
- >2500 => Hard

---

# Epic 4: Workout & Exercise Catalog Service

## Endpoints

### Workouts
- GET /workouts
- GET /workouts/{id}
- GET /workouts/by-plan/{planId}
- GET /workouts/category/{categoryName}
- POST /workouts/{id}/start

### Exercises
- GET /exercises
- GET /exercises/{id}

### Workout Plans
- GET /workout-plans
- GET /workout-plans/{planId}

Workout Categories:
- full-body
- chest
- arms
- shoulders
- back
- legs
- stomach

Browse Workouts:
- Supports page
- pageSize
- category
- difficulty
- duration
- search

Start Workout Session:
- Create WorkoutSessions row.
- Status = Active.
- Return sessionId.
- Idempotent if active session already exists.

---

# Epic 5: Progress Tracking Service

Endpoints:
- POST /progress/workouts
- POST /progress/weight
- GET /progress
- GET /progress/{userId}
- GET /progress/weight-history/{userId}
- GET /progress/achievements
- GET /progress/stats/{userId}

Events:
- weight_updated

Rules:
- Weight range: 40–200kg.
- Achievement unlock publishes achievement_earned event.
- Completing workout updates streaks and statistics.

---

# Epic 6: Nutrition Service

Endpoints:
- GET /nutrition/recommendations
- GET /nutrition/recommendations/{userId}
- GET /nutrition/meals/{id}
- GET /nutrition/meal-plans
- GET /nutrition/meal-plans/by-calories

Rules:
- Nutrition service synchronously reads CalorieTarget from FCE.
- Match meals using calorie ranges and filters.

---

# Epic 7: Smart Coach Service

Endpoints:
- POST /smart-coach/chat
- GET /smart-coach/history
- GET /home

Rules:
- Free users limited to 5 AI messages per 24 hours.
- Home feed uses cache-first strategy.
- Cache expiry = 30 minutes.

---

# Epic 8: Subscription Service

Endpoints:
- GET /subscription/status/{userId}
- POST /subscription/upgrade
- POST /subscription/cancel

Events:
- subscription_upgraded

Tiers:
- Free
- Premium

---

# Epic 9: Notification Service

Endpoints:
- GET /notifications
- PUT /notifications/{id}/read

Events consumed:
- achievement_earned

---

# Workout Service Database Schema

## WorkoutPlans
- Id
- ExternalPlanId
- Name
- Description
- Goal
- Status
- Difficulty

## Workouts
- Id
- WorkoutPlanId
- Name
- Category
- Difficulty
- DurationInMinutes
- CaloriesBurn
- ImageUrl
- IsPremium

## Exercises
- Id
- Name
- Description
- VideoUrl
- TargetMuscles
- EquipmentNeeded
- Difficulty

## WorkoutExercises
- Id
- WorkoutId
- ExerciseId
- OrderIndex
- SetsDefault
- RepsDefault
- RestTimeInSeconds

## WorkoutSessions
- SessionId
- UserId
- WorkoutId
- StartedAt
- Status (Active, Completed, Abandoned)

---

# Global Backend Rules

JWT:
- All protected endpoints require Bearer token.
- Invalid token => AUTH_TOKEN_INVALID.

Response Envelope:
{
  isSuccess,
  message,
  data,
  errors,
  statusCode,
  timestamp
}

Pagination:
- page default = 1
- pageSize default = 20

Rate Limits:
- Anonymous = 50/hr
- Free = 500/hr
- Premium = 2000/hr

Async Events:
- weight_updated
- subscription_upgraded
- achievement_earned

Service Calls:
- Fail gracefully with 503 SRV_SERVICE_UNAVAILABLE.
- Event consumers must be idempotent.
