using UnityEngine;

public class SM : MonoBehaviour {
    private static SM sm;

    // 사운드는 여러 씬에서 쓰이므로 씬에 SM이 없으면 처음 부를 때 자동으로 만든다 (GM, LM, AM과 다른 점)
    public static SM ins {
        get {
            if (sm == null) {
                sm = FindFirstObjectByType<SM>();
                if (sm == null) {
                    sm = new GameObject("SM").AddComponent<SM>();
                }
            }
            return sm;
        }
    }

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource playerSource;
    public AudioSource levelUpSource;

    private void Awake() {
        if (sm == null) {
            sm = this;
        } else if (sm != this) {
            // 두개 이상의 사운드 매니저가 존재하지 않도록 위함
            Destroy(gameObject);
            return;
        }

        if (bgmSource == null) { bgmSource = CreateSource(); }
        if (sfxSource == null) { sfxSource = CreateSource(); }
        if (playerSource == null) { playerSource = CreateSource(); }
        if (levelUpSource == null) { levelUpSource = CreateSource(); }
    }

    private AudioSource CreateSource() {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        return newSource;
    }

    // BGM 재생 (반복). 같은 소스를 쓰므로 이전 BGM은 바뀐다
    public void Bgm(AudioClip clip, float volume = 1f) {
        bgmSource.volume = volume;
        bgmSource.loop = true;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // 플레이어 소리 (점프, 사망) : 같은 스피커라 새 소리가 이전 소리를 끊음 (원래 Player의 AudioSource와 동일)
    public void PlayerSound(AudioClip clip, float volume = 1f) {
        Speaker(playerSource, clip, volume);
    }

    // 레벨업 소리 : 다시 울리면 처음부터 (원래 LM의 AudioSource와 동일)
    public void LevelUpSound(AudioClip clip, float volume = 1f) {
        Speaker(levelUpSource, clip, volume);
    }

    // 지정한 스피커에서 재생 : 이전 소리는 끊기고 새 소리가 처음부터 (source.clip = clip; source.Play(); 와 동일)
    private void Speaker(AudioSource source, AudioClip clip, float volume) {
        if (clip == null) { return; }
        source.volume = volume;
        source.clip = clip;
        source.Play();
    }

    // 효과음 재생 (겹쳐서 재생 가능, 기존 PlayOneShot과 동일)
    public void Sfx(AudioClip clip, float volume = 1f) {
        if (clip == null) { return; }
        sfxSource.PlayOneShot(clip, volume);
    }

    // 특정 위치에서 효과음 재생 (3D 위치 기반 → 거리에 따라 소리 크기가 달라짐)
    public void SfxAt(AudioClip clip, Vector3 position, float volume = 1f) {
        if (clip == null) { return; }
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
