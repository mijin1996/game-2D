# 요구사항 · 로드맵 — BunnyRunning

> 문서 세트: [목차](Proposal.md) · 기획: [컨셉](Concept.md) · [시스템](System.md) · [전투·개체](Combat_Entity.md) · [레벨 디자인](LevelDesign.md) · [밸런스](Balance.md) · 사업·운영: [사업 검토](Business.md) · **요구사항·로드맵** · [Business Model](Monetization.md) · [서버·데이터](Server_Data.md) · [리소스](Resources.md) · [로그·지표](Analytics.md) · [QA·출시](QA_Release.md) · [라이브 운영](LiveOps.md) · [사업화·법무](Business_Legal.md) · [needFix](needFix.md)

> **문서 정보** · 문서 번호 `D02-REQ` · 버전 v1.0 · 상태 `Draft` · 담당 기획자 · 최종 수정 2026-11-21 · 관련 문서 [needFix](needFix.md)

**변경 이력**

| 버전 | 날짜 | 변경 내용 | 사유 | 결정자 | 영향 범위 |
|---|---|---|---|---|---|
| v1.0 | 2026-11-21 | 최초 작성 | — | 기획자 | — |

---

## 1. 이 문서의 역할
**무엇을 · 언제까지 · 어떤 기준으로 완료**할지 정하는 문서예요.
- 우선순위: `Must` · `Should` · `Could` · `Won't`
- 마일스톤: **알파**(기록 루프 정상화) → **베타**(레벨 체감 · 광고) → **출시**(앱인토스)
- 공수는 1인 개발 추정이에요.

## 2. 이미 구현된 것
| ID | 기능 | 근거 문서 | 상태 |
|---|---|---|---|
| `REQ_001` | 자동 스크롤 · 발판 생성 (bag 방식) | [시스템](System.md) · [레벨 디자인](LevelDesign.md) | 완료 |
| `REQ_002` | 누르는 시간 점프 (최대 5회) | [전투 · 개체](Combat_Entity.md) §2 | 완료 ([S-06](needFix.md) 조정 예정) |
| `REQ_003` | 당근 던지기 · 달팽이 처치 | [전투 · 개체](Combat_Entity.md) §3 · §4 | 완료 |
| `REQ_004` | 코인 · 레벨업 (코인 5개마다) | [밸런스](Balance.md) §5 | 완료 |
| `REQ_005` | 광고 매니저 `AM` (토스 SDK, 테스트 ID) | [시스템](System.md) §3-7 | 코드만 있음 (호출 없음) |

## 3. 출시까지 할 일
| ID | 기능 | 우선순위 | 마일스톤 | 공수 | 관련 | 상태 |
|---|---|---|---|---|---|---|
| `REQ_101` | 최고 점수 저장 · SCORE / BEST 표시 | Must | 알파 | 0.5일 | [S-02](needFix.md) | 대기 |
| `REQ_102` | 버그: `Coin.coinCnt` static 초기화 · 씬 이동 중복 잠금 | Must | 알파 | 0.5일 | [S-04](needFix.md) · [S-05](needFix.md) | 대기 |
| `REQ_103` | 당근 쿨다운 0.4초 + 버튼 게이지 · 당근 손맛 | Must | 알파 | 1일 | [S-03](needFix.md) · [S-15](needFix.md) | 대기 |
| `REQ_104` | 2단 점프 + 코요테 타임 · 모서리 여유 | Must | 알파 | 1일 | [S-06](needFix.md) · [S-16](needFix.md) | 대기 |
| `REQ_105` | 레벨업 체감 (속도 · 달팽이 배치 · 발판 높이) | Must | 베타 | 2일 | [S-07](needFix.md) | 대기 |
| `REQ_106` | 달팽이 처치 점수 | Should | 베타 | 0.5일 | [S-08](needFix.md) | 대기 |
| `REQ_107` | 광고 실제 호출 (`AM` 유지 · 게임오버 · 배너) | Must | 베타 | 1일 | [S-01](needFix.md) | 대기 |
| `REQ_108` | 보상형 이어하기 1회 | Should | 베타 | 1일 | [S-11](needFix.md) · [Business Model](Monetization.md) | 대기 |
| `REQ_109` | 광고 빈도 규칙 (첫 1 ~ 2판 없음, 연속 노출 금지) | Must | 베타 | 0.5일 | [S-17](needFix.md) | 대기 |
| `REQ_110` | 로그 이벤트 심기 | Must | 베타 | 1일 | [로그](Analytics.md) | 대기 |
| `REQ_111` | 광고 ID를 테스트 → 실제 ID로 교체 (빌드 설정으로 분리) | Must | 출시 | 0.5일 | [Business Model](Monetization.md) §5 | 대기 |
| `REQ_112` | 출석 코인 · 일일 미션 2 ~ 3개 | Could | 출시 후 | 3일 | needFix §7 | 대기 |
| `REQ_113` | 개인정보처리방침 · 고지 화면, 개발용 패키지 제거 | Must | 출시 | 0.5일 | [사업화 · 법무](Business_Legal.md) · [F-06](needFix.md) | 대기 |
| `REQ_201` | 몬스터 2종 · 기믹 3종 | Won't | — | — | [S-09](needFix.md) · [S-10](needFix.md) | 보류 |
| `REQ_202` | 스킨 상점 · 계절 테마 | Won't | — | — | [S-11](needFix.md) · §6 | 보류 |

## 4. 완료 기준 (Given / When / Then)
| ID | 완료 기준 |
|---|---|
| `REQ_101` | Given 최고 점수 120 / When 이번 판 150으로 끝나면 / Then BEST 150으로 저장되고 앱을 다시 켜도 150이다 |
| `REQ_104` | Given 발판 끝을 지나 0.1초 안 / When 점프를 누르면 / Then 점프가 된다. 공중 점프는 1번만 된다 |
| `REQ_107` | Given 실제 기기(토스 앱) / When 3번째 게임오버 / Then 전면 광고가 1번 뜨고, 닫으면 게임오버 창으로 돌아온다 |
| `REQ_108` | Given 게임오버 / When 이어하기 광고를 끝까지 보면 / Then 같은 점수로 이어서 달리고, 한 판에 1번만 쓸 수 있다 |
| `REQ_109` | Given 앱을 켠 뒤 첫 2판 / When 죽으면 / Then 전면 광고가 뜨지 않는다 |

## 5. 마일스톤
| 마일스톤 | 들어가는 것 | 통과 조건 |
|---|---|---|
| 알파 | REQ_101 ~ 104 | 한 판 30 ~ 45초, 버그 없음 |
| 베타 | REQ_105 ~ 110 | 테스트 광고가 규칙대로 뜸, 로그 수집 |
| 출시 | REQ_111, 113 + [QA · 출시](QA_Release.md) | 앱인토스 심사 통과 |
