using UnityEngine;

using UnityEngine.UI;

using TMPro;



namespace Assets.SimpleLocalization

{

	/// <summary>

	/// Localize text component.

	/// </summary>

    //[RequireComponent(typeof(Text))]

    public class LocalizedText : MonoBehaviour

    {

        public string LocalizationKey;



        public void Start()

        {

            Localize();

            LocalizationManager.LocalizationChanged += Localize;

        }



        public void OnDestroy()

        {

            LocalizationManager.LocalizationChanged -= Localize;

        }



        private void Localize()

        {
            try{
                GetComponent<Text>().text = LocalizationManager.Localize(LocalizationKey);
            }
            catch{
                GetComponent<TMP_Text>().text = LocalizationManager.Localize(LocalizationKey);

            }
            
        }

    }

}