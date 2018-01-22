using UnityEngine;
using System.Collections.Generic;
using System.Linq;
/**
 * Script to move the droid camera with voice commands
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class CentinelSpeechRecognition : MonoBehaviour
{
    private string[] m_Keywords = { "desactivar", "adelante", "retrocede", "derecha", "izquierda", "para", "activar" };

    private SpeechRecognizerManager _speechManager = null;
    private bool _isListening = false;
    int command = 0;
    bool operative = false;
    float speed = 5.0f;

    Vector3 startingPoint;

    #region MONOBEHAVIOUR

    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Reconocimiento de voz solo disponible en plataformas Android.");
            return;
        }

        if (!SpeechRecognizerManager.IsAvailable())
        {
            Debug.Log("Reconocimiento de voz no disponible en este dispositivo.");
            return;
        }

        // We pass the game object's name that will receive the callback messages.
        _speechManager = new SpeechRecognizerManager(gameObject.name);
        startingPoint = transform.position;
    }

    void Update()
    {
            if (!_isListening)
            {
                _isListening = true;
                _speechManager.StartListening(3, "es-ES"); // Use spanish and return maximum three results.
                                                          
            }
            /*
			 * Speech captured so far will be recognized as if the user had stopped speaking at this point. 
			 * it will automatically stop the recognizer listening when it determines speech has completed.
			 */
        //}

        //Selector de acción en cada frame
        switch (command)
        {
            case 0:
                if (!aproximado(transform.position, startingPoint))
                    returnToStart();
                operative = false;
                break;
            case 1:
                if(operative)
                    transform.position += transform.forward * Time.deltaTime * speed;
                break;
            case 2:
                if (operative)
                    transform.position -= transform.forward * Time.deltaTime * speed;
                break;
            case 3:
                if (operative)
                    transform.Rotate(Vector3.up * Time.deltaTime * speed * 5);
                break;
            case 4:
                if (operative)
                    transform.Rotate(Vector3.down * Time.deltaTime * speed * 5);
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        if (_speechManager != null)
            _speechManager.Release();
    }

    #endregion

    #region SPEECH_CALLBACKS

    void OnSpeechEvent(string e)
    {
        switch (int.Parse(e))
        {
            case SpeechRecognizerManager.EVENT_SPEECH_READY:
                Debug.Log("Listo para escuchar");
                break;
            case SpeechRecognizerManager.EVENT_SPEECH_BEGINNING:
                Debug.Log("El usuario ha comenzado a hablar");
                break;
            case SpeechRecognizerManager.EVENT_SPEECH_END:
                Debug.Log("El usuario ha dejado de hablar");
                break;
        }
    }

    void OnSpeechResults(string results)
    {
        _isListening = false;

        // Need to parse
        string[] texts = results.Split(new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
        Debug.Log("Frases reconocidas:\n   " + string.Join("\n   ", texts));

        //INSERTAR COMPARACIONES KEYWORDS
        int i = 0;
        bool found = false;
        while(i < m_Keywords.Length && !found)
        {
			if(texts.Contains(m_Keywords[i])){
                found = true;
                command = i;
            }
            else i++;
        }
        switch (command)
        {
            case 0:
                operative = false;
                break;
            case 6:
                operative = true;
                break;
            default:
                break;
        }
    }

    void returnToStart()
    {
        var p1 = new Vector3(startingPoint.x, startingPoint.y, startingPoint.z);
        var p2 = new Vector3(transform.position.x, startingPoint.y, transform.position.z);
        float dot = Vector3.Dot(transform.forward, (p1 - p2).normalized);

        if (dot < 0.99f && (Mathf.Abs(p1.x - p2.x) > 0.1f || Mathf.Abs(p1.z - p2.z) > 0.1f))
        {
            Vector3 lTargetDir = startingPoint - transform.position;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.deltaTime * speed * 5);
        }
        else if (Mathf.Abs(p1.x - p2.x) > 0.1f || Mathf.Abs(p1.z - p2.z) > 0.1f) {
            transform.position += transform.forward * (Time.deltaTime * speed);
        }
    }

    bool aproximado(Vector3 a, Vector3 b)
    {
        if (Mathf.Abs(a.x - b.x) > 0.1f)
            return false;
        else if (Mathf.Abs(a.y - b.y) > 0.1f)
            return false;
        else if (Mathf.Abs(a.z - b.z) > 0.1f)
            return false;
        return true;
    }

    void OnSpeechError(string error)
    {
        switch (int.Parse(error))
        {
            case SpeechRecognizerManager.ERROR_AUDIO:
                Debug.Log("Error durante la grabación.");
                break;
            case SpeechRecognizerManager.ERROR_CLIENT:
                Debug.Log("Error en los servidores.");
                break;
            case SpeechRecognizerManager.ERROR_INSUFFICIENT_PERMISSIONS:
                Debug.Log("Permisos insuficientes. Se han añadido los permisos RECORD_AUDIO e INTERNET al manifiesto?");
                break;
            case SpeechRecognizerManager.ERROR_NETWORK:
                Debug.Log("Error de red. Asegúrate de que el dispositivo tenga acceso a internet.");
                break;
            case SpeechRecognizerManager.ERROR_NETWORK_TIMEOUT:
                Debug.Log("Expiró el tiempo de respuesta.Asegúrate de que el dispositivo tenga acceso a internet.");
                break;
            case SpeechRecognizerManager.ERROR_NO_MATCH:
                Debug.Log("Sin resultados.");
                break;
            case SpeechRecognizerManager.ERROR_NOT_INITIALIZED:
                Debug.Log("El reconocimiento de voz no está inicializado.");
                break;
            case SpeechRecognizerManager.ERROR_RECOGNIZER_BUSY:
                Debug.Log("El servicio de reconocimiento de voz está ocupado.");
                break;
            case SpeechRecognizerManager.ERROR_SERVER:
                Debug.Log("El servidor devuelve error.");
                break;
            case SpeechRecognizerManager.ERROR_SPEECH_TIMEOUT:
                Debug.Log("No se ha detectado sonido.");
                break;
            default:
                break;
        }

        _isListening = false;
    }

    #endregion
}
