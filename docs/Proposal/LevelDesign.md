# 레벨 디자인 — Main 씬

> 문서 세트: [목차](Proposal.md) · [컨셉](Concept.md) · [시스템](System.md) · [전투·개체](Combat_Entity.md) · **레벨 디자인** · [밸런스](Balance.md) · [Suggestions](Suggestions.md)
> 데이터: [`Balance/platform.json`](Balance/platform.json)

---

## 1. 구성
| 항목 | 내용 | 근거 |
|---|---|---|
| 씬 | `Main` | 빌드 설정 |
| 구조 | 공중 발판이 오른쪽(x = 20)에서 생겨 왼쪽으로 흐름, 아래는 'Dead' 영역 | `PlatformSpawner`, `Player` |
| 배경 | 무한 스크롤 | `BackgroundLoop`, `ScrollingObject` |
| 목표 | 코인을 모으며 오래 달리기 | `GM`, `LM` |

**[구성도] Main 씬 구성**
```mermaid
flowchart LR
  subgraph L1["① 입력 · 씬"]
    S["🎬 Main"]:::scene
  end
  subgraph L2["② 게임 로직 (스크립트)"]
    PL["📜 UniRun_Run_0 (1)<br/>Player · 점프 힘 600"]:::script
    SP["📜 PlatformSpawner<br/>1.25 ~ 1.35초마다"]:::script
    MG["📜 GM · LM · SM · SCM"]:::script
  end
  subgraph L3["③ 게임 오브젝트"]
    SN["📦 달팽이 발판 x3<br/>SnailPatrol"]:::obj
    CO["📦 코인 발판 x3<br/>코인 5개씩"]:::obj
    BG["📦 배경<br/>BackgroundLoop · ScrollingObject"]:::obj
  end
  subgraph L4["④ 화면 · 소리"]
    UI["🖥️ scoreText · levelText<br/>AttackButton · gameoverUI"]:::ui
  end
  S --> PL
  S --> SP
  SP --> SN
  SP --> CO
  S --> BG
  S --> MG
  S --> UI
  classDef scene fill:#2F8F6B,color:#fff,stroke:#2F8F6B
  classDef script fill:#3AA6C8,color:#fff,stroke:#3AA6C8
  classDef obj fill:#6B7280,color:#fff,stroke:#6B7280
  classDef ui fill:#5A4A9E,color:#fff,stroke:#5A4A9E
  classDef data fill:#A33A33,color:#fff,stroke:#A33A33
```
> 읽는 법: 상자 = 씬 · 스크립트 · 오브젝트, 화살표 위 글 = 보내는 신호나 하는 일이에요. ①②③은 일어나는 순서예요.

## 2. 발판 배치 (옆에서 본 모습)
```
 y
 1.0 ┤                       ▀▀▀▀▀▀▀▀ (코인)          ← 발판 높이 범위
     │      🐰                                ▀▀▀▀▀▀▀▀ 🐌
-3.5 ┤ ▀▀▀▀▀▀▀▀                                         ← (가장 낮은 발판)
     │░░░░░░░░░░░░░░ 떨어지면 게임오버 (Dead 영역) ░░░░░░░░
     └──────────────────────────────────────────────────→ x
        플레이어 고정               발판 생성 x = 20 →  ← 10 m/s로 다가옴
```
| 요소 | 값 |
|---|---|
| 발판 생성 위치 | x = 20, 높이 -3.5 ~ 1 랜덤 (씬 값) |
| 발판 생성 간격 | 1.25 ~ 1.35초 (씬 값, 코드 기본값 2.25) |
| 발판 너비 (콜라이더) | 8.41 |
| 발판 종류 | 달팽이 발판 / 코인 발판 (각각 풀 3개) |

**발판 사이 거리**
```
[=========A=========]                     [=========B=========]
|<------ 8.41 ----->|<--- gap 4.1~5.1 --->|
|<------------- 12.5 ~ 13.5 ------------->|

A 시작 ~ B 시작 = 생성 간격(1.25 ~ 1.35초) x 스크롤 10 m/s
빈 틈 = 그 거리 - 발판 너비 8.41
```

**[막대그래프] 발판 사이 빈 틈 vs 1회 점프 수평 이동**
```mermaid
xychart-beta
  title "발판 사이 빈 틈 vs 1회 점프 수평 이동"
  x-axis ["가장 좁은 틈", "가장 넓은 틈", "1회 점프"]
  y-axis "거리" 0 --> 14
  bar [4.1, 5.1, 12]
```
> 읽는 법: 1회 점프(12)가 가장 넓은 틈(5.1)보다 길어요.

## 3. 스폰 테이블 — bag 방식
주머니에 [달팽이 1, 코인 1]을 넣고 섞어서 하나씩 꺼냄 → 다 꺼내면 다시 채움.

| 항목 | 값 |
|---|---|
| bag 구성 | 달팽이 1 : 코인 1 |
| 달팽이 비율 | 50% |
| 같은 종류 최대 연속 | 2 |

```
result : S C | C S | S C | C S
               ^^^^^ 같은 종류는 최대 2번 연속
```

**[원그래프] bag 구성 (현재)**
```mermaid
pie title bag 구성 (현재)
  "달팽이 발판" : 1
  "코인 발판" : 1
```
> 읽는 법: 조각이 클수록 비율이 커요.

## 4. 난이도
현재는 레벨이 올라도 스크롤 속도 · 발판 구성이 그대로예요. 레벨업은 글자 · 소리 · 이펙트만 바뀌어요.

| 레벨 | 필요 코인 (누적) | 스크롤 속도 | bag (달팽이 : 코인) |
|---|---|---|---|
| 모든 레벨 | 5개마다 +1 | 10 | 1 : 1 |

> 레벨에 따라 어려워지는 곡선은 [S-07](Suggestions.md)
