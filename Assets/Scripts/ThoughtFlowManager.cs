using UnityEngine;

public class ThoughtFlowManager : MonoBehaviour
{
    public string[] Thoughts;
    private int currentThought = 0;
    private bool running = false;

    // Update is called once per frame
    void Update() {
        if (running && !ThoughtManager.thinking) {
            Debug.Log("Thought " + currentThought + " of " + Thoughts.Length);
            if (currentThought < Thoughts.Length) {
                ThoughtManager.ShowThought(Thoughts[currentThought]);
                currentThought++;
            } else {
                Destroy(gameObject);
            }
        }
    }

    public void Run() {
        Debug.Log("ThoughtFlowManager started");
        running = true;
    }

}
