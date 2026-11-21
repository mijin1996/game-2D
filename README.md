# 🐰 BunnyRunning (game-2D)

> 한 줄 소개: (직접 작성)
> 예) 토끼 후드를 쓴 러너가 봄 들판을 달리며 코인을 먹고 당근을 던져 달팽이를 물리치는 2D 러너 게임

---

## 📜 개발 과정 히스토리

### Phase 1 — 기초 뼈대 (교수님 수업 · `2D-초기스크립트/`)
수업에서 짠 초기 스크립트 5개(`GM`, `Player`, `Platform`, `PlatformSpawner`, `BackgroundLoop`)로 **기본 게임 루프와 충돌 처리**를 만들었다.

| 사용한 개념 | 적용된 곳 |
|---|---|
| 싱글톤 게임 매니저 (`GM.gm`) | 점수 추가, 게임오버 UI, 클릭 시 씬 다시 불러오기 |
| 오브젝트 풀링 | 발판 3개를 미리 만들어 두고 화면 밖 대기소 ↔ 스폰 위치로 돌려 씀 |
| 무한 배경 스크롤 | 배경이 가로 길이만큼 지나가면 오른쪽 끝으로 재배치 |
| Rigidbody2D 점프 | 2단 점프, 버튼을 빨리 떼면 낮게 점프 |
| 충돌 법선(normal)으로 착지 판정 | `contacts[0].normal.y > 0.7` 이면 바닥 |
| 태그 기반 충돌 | `Player`(발판 밟으면 +1점), `Dead`(사망) |

### Phase 2 — 기능 확장 · 리팩토링 (ME)

**① 매니저 분리 (역할별 싱글톤, 접근은 `ins` 프로퍼티로 통일)**
- `GM` 게임 상태·점수 / `SM` 사운드(BGM·효과음·플레이어·레벨업 채널 분리) / `LM` 레벨(코인 5개마다 레벨업) / `SCM` 씬 전환·게임 가이드 / `AM` 광고(앱인토스 배너·전면 광고, 3번 죽을 때마다)

**② 입력 분리**
- `PlayerInput` 따로 만들어 새 Input System 사용 — PC는 키보드(Space 점프, A 공격), 모바일은 터치

**③ 인게임 콘텐츠 추가**
- 공격: 당근 투사체(`CarrotProjectile`) → `IDamageable` 인터페이스로 맞은 대상에 데미지
- 적: 발판 위를 왕복하는 달팽이(`SnailPatrol`)
- 코인: 먹으면 점수 + 레벨 게이지 + 떠오르는 숫자(Floating Text)
- 발판 스폰: 하나였던 발판을 **달팽이 발판 / 코인 발판 2종 풀**로 나누고, 섞인 주머니(bag) 방식으로 "랜덤이지만 규칙적인" 배치

**④ 연출 · UI**
- `IEffect` 인터페이스로 효과 통일: `PulseEffect`(점수), `BlinkEffect`(레벨), `FloatEffect`, `PressRippleEffect`(버튼)
- 점프·레벨업·게임오버 파티클, 씬별 BGM, 게임오버 시 최고 점수 표시, 가로 화면 고정(`LandscapeMode`)

**⑤ 출시 준비**
- itch.io / Game Jolt 스토어 이미지·소개글 (`StoreImages/`)

#### 💭 고민한 점 · 배운 점
- (직접 작성)
- (직접 작성)

---

## 🛠 기술 스택
- **Language:** C#
- **Engine:** Unity 6 (6000.5.3f1)
- **그 외:** Input System, Apps in Toss SDK(광고), Floating Text Engine Lite, Cartoon FX Remaster

---

## 📁 문서
- [`docs/Proposal/Index.md`](docs/Proposal/Index.md) — 기획서 (컨셉 ~ 사업화 · 법무) · [needFix](docs/Proposal/needFix.md)
- [`docs/MeMemo.md`](docs/MeMemo.md) — 동기/비동기(코루틴·Awaitable·UniTask) 정리 메모
