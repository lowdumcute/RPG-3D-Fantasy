using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInfo : MonoBehaviour
{
    [SerializeField] private UIStatsRadarChart uiStatsRadarChart;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Sprite AvatarImage; // Ảnh hiển thị trên UI
    [SerializeField] private GameObject Avatar; // GameObject chứa Image cần thay đổi
    [SerializeField] private AudioClip clickSound; // Âm thanh khi click
    [SerializeField] private float fadeDuration = 0.5f; // Thời gian làm rõ ảnh
    private AudioSource audioSource;

    private void Start()
    {
        // Kiểm tra và thêm AudioSource nếu chưa có
        audioSource = Avatar.GetComponent<AudioSource>();
    }

    public void ChangeStat()
    {
        Stats stats = new Stats(playerStats.DAttack, playerStats.DDefense, playerStats.DSpeed, playerStats.DMana, playerStats.DHealth);
        uiStatsRadarChart.SetStats(stats);
        ChangeAvatar(AvatarImage);
    }

    public void ChangeAvatar(Sprite newSprite)
    {
        if (Avatar != null)
        {
            Image avatarImg = Avatar.GetComponent<Image>(); // Lấy component Image từ Avatar
            if (avatarImg != null && newSprite != null)
            {
                // Thay đổi ảnh mới
                avatarImg.sprite = newSprite;
                StartCoroutine(FadeInImageAlpha(avatarImg));
                

                PlayClickSound(clickSound); // Phát âm thanh
            }
            else
            {
                Debug.LogWarning("Avatar không có component Image hoặc newSprite bị null!");
            }
        }
        else
        {
            Debug.LogWarning("Avatar GameObject bị null!");
        }
    }

    private IEnumerator FadeInImageAlpha(Image avatarImg)
    {
        // Reset alpha về 0 trước khi bắt đầu
        Color originalColor = avatarImg.color;
        avatarImg.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Đặt alpha = 0

        // Tạo màu mục tiêu với alpha = 1
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        float elapsedTime = 0f;

        // Từ alpha = 0 (mờ) đến alpha = 1 (rõ)
        while (elapsedTime < fadeDuration)
        {
            avatarImg.color = Color.Lerp(avatarImg.color, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo màu ảnh cuối cùng là màu với alpha = 1
        avatarImg.color = targetColor;
    }


    private void PlayClickSound(AudioClip newClickSound)
    {
        if (audioSource != null && newClickSound != null)
        {
            audioSource.Stop(); // Dừng âm thanh hiện tại
            clickSound = newClickSound; // Thay đổi AudioClip
            audioSource.PlayOneShot(clickSound); // Phát âm thanh mới
        }
        else
        {
            Debug.LogWarning("Thiếu AudioSource hoặc AudioClip!");
        }
    }

}
