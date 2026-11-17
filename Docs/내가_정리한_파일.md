### 동기 
- `(Task.Delay(3000).Wait()) → 15초`

### 비동기
- `IEnumerator`/`yield return new WaitForSeconds(1f);`
  - 코루틴은 자동으로 멈춰
  - 값을 돌려받을 수없음
  - 오브젝트가 꺼지거나 파괴되면 자동으로 멈춤
- ##### Awaitable: 유니티 6 기본 내장 =>
  - 자동 멈춤이 안되서 `destroyCancellationToken`를 써야함
- ##### UniTask: 외부 라이브러리 = 메모리 거의 안 씀


- await만 붙이기 → 15초
- 먼저 시작해 두기 (`Task<Egg> eggsTask = FryEggsAsync(2);`) → 6초
- 합치기 + 예외 (`MakeToastWithButterAndJamAsync`) = 구조 정리

- async/await는 `try/catch`로 에러 잡기

- WhenAny: 끝나는 대로 하나씩 처리
- WhenAll: 전부 끝나면 한 번에 처리


//명령어 git pull origin master --allow-unrelated-histories --no-edit
