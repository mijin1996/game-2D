# 시스템 — 러너 · 발판 · 성장 · 광고

> 문서 세트: [목차](../Index.md) · 기획: [컨셉](Concept.md) · **시스템** · [전투·개체](Combat_Entity.md) · [레벨 디자인](LevelDesign.md) · [밸런스](Balance.md) · 사업·운영: [사업 검토](../대표/Business.md) · [요구사항·로드맵](Requirements.md) · [Business Model](Monetization.md) · [서버·데이터](../개발/Server_Data.md) · [리소스](../디자인/Resources.md) · [로그·지표](Analytics.md) · [QA·출시](../QA/QA_Release.md) · [라이브 운영](../운영/LiveOps.md) · [사업화·법무](../대표/Business_Legal.md) · [needFix](../needFix.md)
> 관련 코드: `Player.cs` · `PlayerInput.cs` · `PlatformSpawner.cs` · `Platform.cs` · `Coin.cs` · `GM.cs` · `LM.cs` · `SM.cs` · `SCM.cs` · `AM.cs`

> **문서 정보** · 문서 번호 `D03-SYSTEM` · 버전 v1.0 · 상태 `Review` · 담당 기획자 · 클라이언트 개발자 · 최종 수정 2026-11-21 · 관련 문서 [전투·개체](Combat_Entity.md) · [Business Model](Monetization.md)

**변경 이력**

| 버전 | 날짜 | 변경 내용 | 사유 | 결정자 | 영향 범위 |
|---|---|---|---|---|---|
| v0.9 | 2026-11-20 | 기존 문서 (자세한 기록은 git 커밋) | — | 기획자 | — |
| v1.0 | 2026-11-21 | 문서 정보 · 변경 이력 · 문서 번호 추가, 사업 · 운영 문서와 연결 | — | 기획자 | 문서만 |

---

## 0. 한눈에 보는 시스템 구조

**[구성도] 스크립트 연결**
```mermaid
flowchart LR
  subgraph L1["① 입력 · 씬"]
    IN["🎬 PlayerInput<br/>Space · 터치 · A"]:::scene
  end
  subgraph L2["② 게임 로직 (스크립트)"]
    P["📜 Player"]:::script
    PS["📜 PlatformSpawner"]:::script
    GM["📜 GM"]:::script
    LM["📜 LM"]:::script
  end
  subgraph L3["③ 게임 오브젝트"]
    CR["📦 당근<br/>CarrotProjectile"]:::obj
    SN["📦 달팽이<br/>SnailPatrol"]:::obj
    PF["📦 발판 x6<br/>달팽이 3 · 코인 3"]:::obj
    CO["📦 코인"]:::obj
  end
  subgraph L4["④ 화면 · 소리"]
    UI["🖥️ Score · LV<br/>게임오버 창"]:::ui
    SM["🖥️ SM<br/>소리 4채널"]:::ui
  end
  subgraph L5["⑤ 데이터 · 저장"]
    AM["💾 AM 광고<br/>(부르는 곳 없음)"]:::data
  end
  IN -->|"① 점프 · 공격"| P
  P -->|"② 당근 생성"| CR
  CR -->|"③ 맞으면 사라짐"| SN
  PS -->|"1.25~1.35초마다 배치"| PF
  PF --> CO
  P -->|"④ 닿음"| CO
  CO -->|"⑤ 점수 +1"| GM
  CO -->|"⑤ 코인 +1"| LM
  LM -->|"5개마다 레벨업"| UI
  P -->|"⑥ Dead 트리거"| GM
  GM -->|"⑦ 게임오버 창"| UI
  GM -->|"⑦ 게임오버 BGM"| SM
  classDef scene fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef script fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef obj fill:#6B7280,color:#fff,stroke:#6B7280
  classDef ui fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
```

## 1. 화면 흐름
빌드 씬: `0 Intro` → `1 Main` (`2 protocal`은 테스트 씬)

**[상태도] 화면 전환**
```mermaid
stateDiagram-v2
  [*] --> Intro
  Intro --> Guide: 가이드 버튼
  Guide --> Intro: 닫기
  Intro --> Main: 시작 (효과음 + 1초)
  Main --> GameOver: 낙사 / 달팽이
  GameOver --> Main: Restart
  GameOver --> Intro: Home
  classDef play fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef wait fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef fail fill:#A33A33,color:#fff,stroke:#A33A33
  classDef stop fill:#6B7280,color:#fff,stroke:#6B7280
  classDef done fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  class Intro,Guide wait
  class Main play
  class GameOver fail
```

**[와이어프레임] 플레이 화면 (기준 640 × 360)**
```
+----------------------------------------------------------------+
|                                                                |
|                                                    ------------|
|                                                    :          :|
|                                                    :     A    :|
|                                                    ------------|
|                 -------------------------------                |
|                 :                             :                |
|                 :              D              :                |
|                 -------------------------------                |
|                                                                |
|                                                                |
|                                                                |
|                                             --------           |
|                   ------------------------------B---           |
|                   :                        :                   |
|                   :            C           :                   |
|                   :                        :                   |
|                   --------------------------                   |
+----------------------------------------------------------------+
```

| 표시 | 요소 | 하는 일 | 씬 이름 · 화면 위치 (x, y 픽셀, 왼쪽 아래 = 0) |
|---|---|---|---|
| A | 레벨 글자 | "LV. n" · 레벨업 때 깜빡임 | `Level` · 524.4 ~ 637.6, 264 ~ 314 |
| B | 공격 버튼 | 당근 던지기 | `AttackButton` · 455.4 ~ 525.4, 90 ~ 110 |
| C | 점수 | 코인 점수 | `Score Text` · 194.9 ~ 443.1, 24 ~ 100.1 |
| D | 게임오버 글자 (게임오버 때만 켜짐) | 게임오버 창 | `Gameover Text (1)` · 172.5 ~ 472.5, 193.2 ~ 242.2 |

점프는 버튼이 없고 **화면 아무 곳이나 터치**해요 (누르는 시간 = 높이).

## 2. 상태 머신

**게임 (`GM`)**
| 상태 | 발판 생성 | 달팽이 | 점수 | BGM |
|---|---|---|---|---|
| Playing | 1.25 ~ 1.35초마다 | 왕복 | 코인마다 +1 | Main 음악 |
| GameOver | 멈춤 | 정지 (애니메이션 끔) | 더 안 쌓임 | 게임오버 음악 |

**[상태도] 러너 (`Player`)**
```mermaid
stateDiagram-v2
  [*] --> Grounded
  Grounded --> Air: 점프 (횟수 1)
  Air --> Air: 공중 점프 (횟수 < 5)
  Air --> Grounded: 위를 향한 면(법선 y > 0.7)에 착지 → 횟수 0
  Grounded --> Dead: 'Dead' 태그 트리거
  Air --> Dead: 'Dead' 태그 트리거
  classDef play fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef wait fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef fail fill:#A33A33,color:#fff,stroke:#A33A33
  classDef stop fill:#6B7280,color:#fff,stroke:#6B7280
  classDef done fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  class Grounded,Air play
  class Dead fail
```

## 3. 작동 로직
### 3-1. 입력 (`PlayerInput`)

> 세부 조건(코드 기준)은 [부록 A-1](#a-1-입력-playerinput)에 있어요.

### 3-2. 점프

**[흐름도] 점프**
```mermaid
flowchart TD
  subgraph s0["① 점프 누름 (Space · 화면 터치)"]
    direction TB
    n1(["▶ 점프 누름"]):::start
    n2{"죽었음?"}:::dec
    n1 --> n2
    n3(["⏹ 무시"]):::endc
    n2 -->|Y| n3
    n4{"점프 횟수 #lt; 5 ?"}:::dec
    n2 -->|N| n4
    n5(["⏹ 무시"]):::endc
    n4 -->|N| n5
    n6["✏️ 점프 횟수 +1<br/>속도를 (0, 0)으로"]:::data
    n4 -->|Y| n6
    n7["⚙️ 위로 힘 600<br/>→ 위 속도 12"]:::proc
    n6 --> n7
    n8[/"📢 점프 소리 · 점프 이펙트"/]:::io
    n7 --> n8
  end
  subgraph s1["② 점프 뗌"]
    direction TB
    n9(["▶ 점프 뗌"]):::start
    n10{"올라가는 중?"}:::dec
    n9 --> n10
    n11(["⏹ 그대로"]):::endc
    n10 -->|N| n11
    n12["✏️ 속도 × 0.5 = 낮은 점프"]:::data
    n10 -->|Y| n12
  end
  subgraph s2["③ 무언가에 닿음"]
    direction TB
    n13(["▶ 충돌 시작"]):::start
    n14{"첫 접점 법선 y #gt; 0.7?<br/>(위에서 밟음)"}:::dec
    n13 --> n14
    n15(["⏹ 옆면 → 착지 아님"]):::endc
    n14 -->|N| n15
    n16["✏️ 착지 · 점프 횟수 0"]:::data
    n14 -->|Y| n16
  end
  s0 ~~~ s1 ~~~ s2
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

> 세부 조건(코드 기준)은 [부록 A-2](#a-2-점프)에 있어요.

### 3-3. 공격

**[흐름도] 당근 공격**
```mermaid
flowchart TD
  n1(["▶ 공격 입력 (A · 공격 버튼)"]):::start
  n2{"죽었음?"}:::dec
  n1 --> n2
  n3(["⏹ 무시"]):::endc
  n2 -->|Y| n3
  n4[/"📢 공격 애니메이션"/]:::io
  n2 -->|N| n4
  n5["⚙️ 당근 생성 → attackTarget 방향<br/>(없으면 오른쪽) 속도 12"]:::proc
  n4 --> n5
  n6{"5초 안에<br/>무언가에 닿음?"}:::dec
  n5 --> n6
  n7(["⏹ 5초 뒤 자동 제거"]):::endc
  n6 -->|N| n7
  n8{"IDamageable?"}:::dec
  n6 -->|Y| n8
  n9(["⏹ 통과"]):::endc
  n8 -->|N| n9
  n10["✏️ Damage(1) → 달팽이 사라짐"]:::data
  n8 -->|Y| n10
  n11[/"📢 충돌 이펙트 0.5초 · 당근 제거"/]:::io
  n10 --> n11
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

```
  B  >>>>>>>>>>>>>>>>>>>>>>>>  @     당근 속도 12, 맞으면 데미지 1 = 달팽이 사라짐
     |<------ 5s x 12 = 60 ------>|  아무것도 안 맞으면 5초 뒤 제거
```

> 세부 조건(코드 기준)은 [부록 A-3](#a-3-공격)에 있어요.

### 3-4. 발판 생성 (오브젝트 풀 + bag)

**[흐름도] 발판 하나가 나오기까지**
```mermaid
flowchart TD
  n1(["▶ 매 프레임"]):::start
  n2{"게임오버?"}:::dec
  n1 --> n2
  n3(["⏹ 멈춤"]):::endc
  n2 -->|Y| n3
  n4{"생성 시간 됨?"}:::dec
  n2 -->|N| n4
  n5["⚙️ 기다림"]:::proc
  n4 -->|N| n5
  n6["✏️ 다음 간격 1.25 ~ 1.35초<br/>높이 -3.5 ~ 1 랜덤"]:::data
  n4 -->|Y| n6
  n7{"bag 비었음?"}:::dec
  n6 --> n7
  n8["✏️ S 1 · C 1 넣고 섞기"]:::data
  n7 -->|Y| n8
  n9["⚙️ bag에서 하나 꺼냄<br/>→ S(달팽이) 또는 C(코인)"]:::proc
  n7 -->|N| n9
  n10["⚙️ 그 종류 풀에서 다음 차례 발판<br/>(S1 → S2 → S3 → S1)"]:::proc
  n9 --> n10
  n11["✏️ 끄기 → (x 20, 높이)로 이동<br/>→ 코인 다시 켬 → 켜기"]:::data
  n10 --> n11
  n12["⚙️ 켜질 때: 장애물 켬<br/>달팽이 위치 랜덤 · 초기화"]:::proc
  n11 --> n12
  n13(["⏹ 왼쪽으로 10 m/s 이동"]):::endc
  n12 --> n13
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

**bag 방식 (섞인 주머니)**
```
1) fill    : [S] [C]
2) shuffle : [C] [S]
3) take one by one -> empty -> back to 1)

result : S C | C S | S C | C S | C S | S C
               ^^^^^
               같은 종류는 최대 2번 연속

S = 달팽이 발판 · C = 코인 발판
```

> 세부 조건(코드 기준)은 [부록 A-4](#a-4-발판-생성-오브젝트-풀--bag)에 있어요.

### 3-5. 코인 · 점수 · 레벨

**[흐름도] 코인을 먹었을 때**
```mermaid
flowchart TD
  n1(["▶ 플레이어가 코인에 닿음"]):::start
  n2{"이미 먹은 코인?"}:::dec
  n1 --> n2
  n3(["⏹ 무시"]):::endc
  n2 -->|Y| n3
  n4[/"📢 +n 글자 · 코인 소리 · 코인 끔"/]:::io
  n2 -->|N| n4
  n5["✏️ GM 점수 +1 (점수 펄스)"]:::data
  n4 --> n5
  n6["✏️ LM 코인 +1"]:::data
  n5 --> n6
  n7{"코인 ≥ 5 ?"}:::dec
  n6 --> n7
  n8(["⏹ 끝"]):::endc
  n7 -->|N| n8
  n9["✏️ 레벨 +1 · 코인 0"]:::data
  n7 -->|Y| n9
  n10[/"📢 #quot;LV. n#quot; 깜빡임 · 레벨업 소리 · 이펙트"/]:::io
  n9 --> n10
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

> 세부 조건(코드 기준)은 [부록 A-5](#a-5-코인--점수--레벨)에 있어요.

### 3-6. 게임오버

**[흐름도] 죽은 순간부터 다시 시작까지**
```mermaid
flowchart TD
  n1(["▶ Dead 트리거에 닿음<br/>(떨어짐 영역 · 달팽이)"]):::start
  n2{"이미 죽음?"}:::dec
  n1 --> n2
  n3(["⏹ 무시"]):::endc
  n2 -->|Y| n3
  n4[/"📢 사망 애니메이션 · 사망 소리"/]:::io
  n2 -->|N| n4
  n5["✏️ 속도 0 · isDead"]:::data
  n4 --> n5
  n6["⚙️ GM.OnPlayerDead()"]:::proc
  n5 --> n6
  n7[/"📢 점수 · 레벨 글자 숨김"/]:::io
  n6 --> n7
  n8[/"📢 #quot;BEST Score: 이번 판 점수#quot; 표시"/]:::io
  n6 --> n8
  n9[/"📢 게임오버 창 · 이펙트 · 공격 버튼 숨김"/]:::io
  n6 --> n9
  n10[/"📢 효과음 · 게임오버 BGM"/]:::io
  n6 --> n10
  n11(["▶ Restart / Home 버튼"]):::start
  n6 --> n11
  n12["⚙️ 클릭음 길이만큼 기다림"]:::proc
  n11 --> n12
  n13(["⏹ 씬 로드"]):::endc
  n12 --> n13
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

> 세부 조건(코드 기준)은 [부록 A-6](#a-6-게임오버)에 있어요.

### 3-7. 광고 (`AM`)

**[흐름도] 광고 (코드에 있는 흐름 · 지금은 호출 없음)**
```mermaid
flowchart TD
  subgraph s0["① 시작"]
    direction TB
    n1(["▶ AM 시작 (Intro 씬)"]):::start
    n2["⚙️ 전면 광고 미리 로드"]:::proc
    n1 --> n2
    n3["✏️ 로드되면 준비 완료"]:::data
    n2 --> n3
  end
  subgraph s1["② OnGameOver() 호출 시 (지금은 부르는 곳 없음)"]
    direction TB
    n4(["▶ OnGameOver()"]):::start
    n5["✏️ 사망 횟수 +1"]:::data
    n4 --> n5
    n6{"준비됨 AND<br/>사망 횟수가 3의 배수?"}:::dec
    n5 --> n6
    n7(["⏹ 표시 안 함"]):::endc
    n6 -->|N| n7
    n8[/"📢 전면 광고 표시"/]:::io
    n6 -->|Y| n8
    n9(["⏹ 닫히면 다음 광고 로드"]):::endc
    n8 --> n9
  end
  subgraph s2["③ ShowBanner() 호출 시 (지금은 부르는 곳 없음)"]
    direction TB
    n10(["▶ ShowBanner()"]):::start
    n11[/"📢 화면 하단에 배너"/]:::io
    n10 --> n11
  end
  s0 ~~~ s1 ~~~ s2
  classDef start fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef proc fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
  classDef dec fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef io fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef endc fill:#6B7280,color:#fff,stroke:#6B7280
```

> 세부 조건(코드 기준)은 [부록 A-7](#a-7-광고-am)에 있어요.

## 4. 예외 처리 (Edge Case) — 현재 동작
**[구성도] 예외 처리 지도**
```mermaid
flowchart TB
  subgraph G0["📢 광고"]
    direction LR
    S1["#1 광고"]:::sit --> R1["⚠️ AM은 Intro 씬에만 · 부르는 곳 없음 (S-01)"]:::bad
    S2["#2 전면 광고 로드 실패"]:::sit --> R2["✅ 경고 로그만 · 게임 진행"]:::ok
  end
  subgraph G1["🐰 점프 · 공격"]
    direction LR
    S7["#7 공격 연타"]:::sit --> R7["⚠️ 쿨다운 없음 (S-03)"]:::bad
    S8["#8 발판 옆면에 부딪힘"]:::sit --> R8["✅ 법선 y ≤ 0.7 → 착지로 안 봄"]:::ok
    S9["#9 발판에서 떨어질 때"]:::sit --> R9["💡 점프 횟수 그대로 → 떨어지며 점프 가능 (S-06)"]:::todo
  end
  subgraph G2["🪙 발판 · 코인"]
    direction LR
    S4["#4 같은 코인 두 번 충돌"]:::sit --> R4["✅ stepped로 1번만"]:::ok
    S5["#5 재사용된 발판의 코인"]:::sit --> R5["✅ 자식 코인 전부 다시 켬"]:::ok
    S6["#6 풀 크기 3"]:::sit --> R6["✅ 4번째 사용은 빨라야 8번째 생성"]:::ok
  end
  subgraph G3["🏁 점수 · 화면 · 기타"]
    direction LR
    S3["#3 최고 점수"]:::sit --> R3["⚠️ 이번 판 점수 표시 · 저장 안 함 (S-02)"]:::bad
    S10["#10 씬 이동 버튼 연타"]:::sit --> R10["⚠️ 막는 장치 없음 (S-04)"]:::bad
    S11["#11 떠오르는 숫자 카운터"]:::sit --> R11["⚠️ static이라 판이 바뀌어도 증가 (S-05)"]:::bad
    S12["#12 사운드 매니저 없음"]:::sit --> R12["✅ SM.ins가 자동 생성"]:::ok
    S13["#13 화면 회전"]:::sit --> R13["✅ 가로 고정"]:::ok
  end
  G0 ~~~ G1
  G1 ~~~ G2
  G2 ~~~ G3
  classDef sit fill:#EE7E1C,color:#fff,stroke:#EE7E1C
  classDef ok fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef bad fill:#A33A33,color:#fff,stroke:#A33A33
  classDef todo fill:#6B7280,color:#fff,stroke:#6B7280
```

| # | 상황 | 현재 동작 | 상태 |
|---|---|---|---|
| 1 | 광고 | `AM`은 Intro 씬에만 있고, 광고 함수를 부르는 곳이 없음 | ⚠️ → [S-01](../needFix.md) |
| 2 | 전면 광고 로드 실패 | 경고 로그만, 게임은 진행 | ✅ |
| 3 | 최고 점수 | "BEST Score"에 이번 판 점수를 표시, 저장 안 함 | ⚠️ → [S-02](../needFix.md) |
| 4 | 같은 코인 두 번 충돌 | `stepped`로 1번만 | ✅ |
| 5 | 재사용된 발판의 코인 | 생성 때 자식 코인 전부 다시 켬 | ✅ |
| 6 | 풀 크기 3 | 같은 종류 4번째 사용은 빨라야 8번째 생성 (밸런스 §4) | ✅ |
| 7 | 공격 연타 | 쿨다운 없음 | ⚠️ → [S-03](../needFix.md) |
| 8 | 옆면에 부딪힘 | 법선 y ≤ 0.7이면 착지로 안 봄 | ✅ |
| 9 | 발판에서 떨어질 때 | 점프 횟수 그대로 → 떨어지면서도 점프 가능 | → [S-06](../needFix.md) |
| 10 | 씬 이동 버튼 연타 | 막는 장치 없음 | ⚠️ → [S-04](../needFix.md) |
| 11 | 떠오르는 숫자 카운터 | `static`이라 판이 바뀌어도 계속 증가 | ⚠️ → [S-05](../needFix.md) |
| 12 | 사운드 매니저 없음 | `SM.ins`가 자동으로 만들어 줌 | ✅ |
| 13 | 화면 회전 | 가로 고정 (`LandscapeMode`) | ✅ |

---

## 부록 A. 세부 조건 (코드 기준)
본문 흐름도의 조건을 코드 그대로 적은 개발 참고용이에요. 기획 내용은 본문에 있어요.

### A-1. 입력 (`PlayerInput`)
```
PC     : 점프 = Space 누른 순간 / 뗀 순간, 공격 = A 누른 순간 (새 Input System)
모바일  : 점프 = 화면 터치 누른 순간 / 뗀 순간, 공격 = AttackButton
```

### A-2. 점프
```
점프 누름
├─ IF 사망 → 무시
├─ IF 점프 횟수 < 5
│   ├─ 횟수 +1
│   ├─ 속도를 (0, 0)으로 → 위로 힘 600 (씬 값, 질량 1 · 물리 간격 0.02초 → 위 속도 12)
│   └─ 점프 소리(플레이어 전용 스피커, 이전 소리를 끊음) · 점프 이펙트(2초)
└─ ELSE → 무시
점프 뗌
└─ IF 올라가는 중 → 속도 × 0.5
착지 (충돌 시작)
└─ IF 첫 접점 법선 y > 0.7 (위에서 밟음) → 바닥, 횟수 0
```

### A-3. 공격
```
공격 입력
├─ IF 사망 → 무시
├─ 공격 애니메이션
└─ 당근 생성 → 조준 대상(attackTarget)이 있으면 그쪽, 없으면 오른쪽으로 속도 12 → 5초 뒤 제거
당근 충돌 → IF IDamageable → Damage(1) → 이펙트(0.5초) → 당근 제거
```

### A-4. 발판 생성 (오브젝트 풀 + bag)
```
시작: 달팽이 발판 3개 · 코인 발판 3개를 화면 밖 대기소(0, -25)에 미리 생성
매 프레임
├─ IF 게임오버 → 멈춤
└─ IF 다음 생성 시간 도달
    ├─ 다음 간격 = 1.25 ~ 1.35초 랜덤, 높이 = -3.5 ~ 1 랜덤 (씬 값)
    ├─ 종류 = bag에서 하나 꺼내기
    │   └─ IF bag 비었음 → [달팽이 × 1, 코인 × 1] 채우고 섞기 (Fisher-Yates)
    ├─ 해당 종류 풀에서 다음 차례 발판 (3개 돌려 쓰기)
    ├─ 끈 상태에서 (x = 20, 높이)로 옮김 → 자식 코인 전부 다시 켬 → 다시 켬
    │   └─ 켜질 때 장애물 활성 (obstacleChance 100%), 달팽이 위치 랜덤 · 상태 초기화
    └─ 발판은 ScrollingObject로 왼쪽 10 m/s 이동
```

### A-5. 코인 · 점수 · 레벨
```
코인에 Player 태그가 닿음
├─ IF 이미 먹음 → 무시
└─ ELSE → 떠오르는 숫자 · GM 점수 +1(점수 글자 펄스) · LM 코인 +1 · 코인 소리 · 코인 끔
LM 코인 ≥ 5 → 레벨 +1, 코인 0, "LV. n" 빨간 깜빡임, 레벨업 소리(전용 스피커), 레벨업 이펙트
```

### A-6. 게임오버
```
'Dead' 태그 트리거(떨어짐 영역 · 달팽이)에 닿음 AND 살아 있음
├─ 사망 애니메이션 · 사망 소리 · 속도 0 · isDead
└─ GM.OnPlayerDead()
    ├─ 점수 · 레벨 글자 숨김, "BEST Score: (이번 판 점수)" 표시
    ├─ isGameover, 게임오버 창 · 이펙트 켬, 공격 버튼 숨김
    └─ 효과음 + BGM을 게임오버 음악으로
```

### A-7. 광고 (`AM`)
```
시작 → 전면 광고 미리 로드 (로드되면 준비 완료)
OnGameOver() 호출 시: 사망 횟수 +1
├─ IF 준비 안 됨 OR 사망 횟수가 3의 배수가 아님 → 표시 안 함
└─ ELSE → 전면 광고 표시 → 닫히면 다음 광고 다시 로드
ShowBanner(): 하단에 배너 / HideBanner()
```
