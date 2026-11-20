# 리소스 (아트 · 사운드) 명세 — BunnyRunning

> 문서 세트: [목차](Proposal.md) · 기획: [컨셉](Concept.md) · [시스템](System.md) · [전투·개체](Combat_Entity.md) · [레벨 디자인](LevelDesign.md) · [밸런스](Balance.md) · 사업·운영: [사업 검토](Business.md) · [요구사항·로드맵](Requirements.md) · [BM·경제](Monetization.md) · [서버·데이터](Server_Data.md) · **리소스** · [로그·지표](Analytics.md) · [QA·출시](QA_Release.md) · [라이브 운영](LiveOps.md) · [사업화·법무](Business_Legal.md) · [needFix](needFix.md)

> **문서 정보** · ID `D09-RES` · v1.0 · 상태 `Draft` · Owner ME (기획 · 개발) · 최종 수정 2026-11-21 · 관련 `D04-ENTITY`, `D13-LEGAL`

**변경 이력**

| 버전 | 날짜 | 변경 내용 | 사유 | 결정자 | 영향 범위 |
|---|---|---|---|---|---|
| v1.0 | 2026-11-21 | 최초 작성 | 사내 문서 기준 v3 적용 | ME | 문서만 |

---

## 1. 이 문서의 역할
게임에 들어가는 **모든 그림 · 소리 · 폰트의 목록 · 규격 · 라이선스**를 관리해요.
> **규칙:** 라이선스가 `확인됨`이 아닌 리소스가 있으면 출시하지 않아요 ([QA · 출시](QA_Release.md)).

## 2. 지금 쓰는 리소스

**캐릭터 · 그림**
| 리소스 ID | 파일 · 폴더 | 쓰는 곳 | 출처 | 라이선스 |
|---|---|---|---|---|
| `SPR_RUNNER` | `UniRun_Run_*` 스프라이트 · `img/Run.anim` · `Jump.anim` · `Die.anim` | 토끼 러너 | 유니티짱 계열 (`img/License/UCL2.0` 동봉) | 🔶 **UCL 2.0** — 상업 이용 가능하지만 **라이선스 표기 의무**가 있어요. 표기 위치(게임 · 스토어)를 원문 PDF로 확인 → [F-03](needFix.md#8-사내-문서-기준-v3-적용-후-정할-것) |
| `SPR_SNAIL` | `img/SnailAni.anim` | 달팽이 | 미확인 | ⚠️ 확인 필요 |
| `SPR_CARROT` | `img/Carrot_Pixel.png` | 당근 | 직접 제작 추정 | ⚠️ 직접 만든 것인지 기록 |
| `UI_BTN_*` | `img/ButtonYellow.png` · `GreenButton.png` | 버튼 | 직접 제작 추정 | ⚠️ 기록 |
| `BG_INTRO` | `img/IntroBackGround.png` | 인트로 배경 | 미확인 | ⚠️ 확인 필요 |
| `FX_PIXEL_*` | `Pixel Art/PixelArtRPGVFXLite` | 레벨업 · 점프 이펙트 | 에셋 팩 (Lite) | ⚠️ 라이선스 원문 저장 |
| `FX_CFXR_*` | `JMO Assets/Cartoon FX Remaster` | 충돌 · 게임오버 이펙트 | Unity Asset Store (JMO) | ⚠️ 다운로드 기록 |
| `FX_*` | `FX/Sprites` · `FX/Prefabs` | 별 파티클 등 | 미확인 | ⚠️ 확인 필요 |
| `UI_FLOATTEXT` | `BlackMassSoftware/FloatingTextEngineLite` | "+n" 떠오르는 숫자 | 에셋 (고지 파일 있음) | ✅ 폰트 SIL OFL 1.1, 코드는 확인 |

**사운드**
| 리소스 ID | 파일 | 쓰는 곳 | 출처 (파일 이름 기준) | 라이선스 |
|---|---|---|---|---|
| `BGM_INTRO` | `intromusic.mp3` | 인트로 | 미확인 | ⚠️ |
| `BGM_MAIN` | `cookey12563-doki-doki-crafting-club-194811.mp3` | 플레이 | 무료 음원 사이트 (Pixabay 이름 형식) | ⚠️ 다운로드 페이지 · 라이선스 저장 |
| `BGM_END` | `endmusic.mp3` · `EndMidtrermSound.mp3` | 게임오버 | 미확인 | ⚠️ |
| `SFX_JUMP` · `SFX_DIE` | `jump.wav` · `die.wav` | 점프 · 사망 | 교재 예제 추정 | ⚠️ |
| `SFX_COIN` | `coinSound5.ogg` | 코인 | 미확인 | ⚠️ |
| `SFX_LEVELUP` | `level-up-431472.mp3` · `fairy sparkle5.mp3` | 레벨업 | Pixabay 이름 형식 · 미확인 | ⚠️ |
| `SFX_UI_CLICK` | `버튼클릭3.wav` | 버튼 | 미확인 | ⚠️ |

**폰트**
| 리소스 ID | 파일 | 라이선스 |
|---|---|---|
| `FONT_MAIN` | `Font/Kenney Mini Square.ttf` | ✅ Kenney 무료 폰트 (`Font/License.txt`) |
| `FONT_DMG` | `Oswald-VariableFont_wght.ttf` | ✅ SIL OFL 1.1 |

**출시 자료 (이미 있음)**
| 리소스 ID | 파일 | 규격 |
|---|---|---|
| `STORE_ITCH_*` | `StoreImages/itch/` (커버 630 × 500 · 스크린샷 5장) | itch.io |
| `STORE_GJ_*` | `StoreImages/GameJolt/` (헤더 2000 × 500 · 썸네일 1920 × 1080 · 스크린샷 5장) | Game Jolt |
| `STORE_AIT_*` | 없음 | 앱인토스 아이콘 · 썸네일 규격 확인 필요 |

## 3. 새로 필요한 리소스
| 리소스 ID | 종류 | 규격 | 관련 | 상태 |
|---|---|---|---|---|
| `UI_BEST` | SCORE / BEST 두 줄 게임오버 창 | 가로 화면 기준 | S-02 | 대기 |
| `UI_CARROT_GAUGE` | 당근 버튼 쿨다운 게이지 | 버튼 위 원형 | S-03 | 대기 |
| `UI_CONTINUE` | "광고 보고 이어하기" 버튼 | — | REQ_108 | 대기 |
| `UI_LICENSE` | 라이선스 표기 화면 (UCL 로고 · 폰트 · 오픈소스) | — | F-03 | 대기 |
| `STORE_AIT_*` | 앱인토스 등록 이미지 | 콘솔 규격 | [QA · 출시](QA_Release.md) | 대기 |

## 4. 규칙
- 무료 에셋 · 음원은 다운로드 페이지 주소 · 날짜 · 라이선스 원문을 `docs/licenses/`에 저장해요.
- 출처를 모르는 리소스는 같은 역할의 확인된 리소스로 바꿔요.
