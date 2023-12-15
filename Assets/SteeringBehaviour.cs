using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    [Range(0, 5)]
    public float maxSpeed = 2;
    Vector3 target;
    float orientation;
    Vector3 velocity;
    float rotation;
    public GameObject player;
    Pathp path;
    int currentNode = 0;

    // Start is called before the first frame update
    void Start()
    {
        path = new Pathp();
        path.AddNode(new Vector3(-20.5f,-14,0));
        path.AddNode(new Vector3(-20.5f,6,0));
        path.AddNode(new Vector3(27,6,0));
        path.AddNode(new Vector3(27,-14,0));
    }

    // Update is called once per frame
    void Update()
    {
        target = player.transform.position;
        pathFollowing();
        //seek(target);
    }

    private bool pathFollowing() {
        Vector3 target = new Vector3(-1000, -1000, -1000);
        if (path != null) {
            List<Vector3> nodes = path.GetNodes();
            target = nodes[currentNode];
            if ( (target - transform.position).magnitude <= 1.1f) {
                currentNode += 1;
                if (currentNode >= nodes.Count) {
                    currentNode = nodes.Count - 1;
                }
            }
        }
        if(target.x != -1000){
            seek(target);
            return true;
        }
        return false;
    }

    private void seek(Vector3 tTarget) {
        if(KinematicArrivep.getSteering(tTarget, transform.position, ref orientation, maxSpeed, 1f) != null){
            KinematicSteeringInfo info = KinematicSeekp.getSteering(tTarget, transform.position, ref orientation, maxSpeed);
            transform.position += info.velocity * Time.deltaTime;
            orientation += info.rotation * Time.deltaTime;
        }
    }
    
}

class KinematicSeekp{
    public static KinematicSteeringInfo getSteering(Vector3 target, Vector3 initialPos, ref float orientation, float maxSpeed){
        KinematicSteeringInfo info = new KinematicSteeringInfo();
        info.velocity = target - initialPos;
        info.velocity.Normalize();
        info.velocity *= maxSpeed;

        orientation = NewOrientation(orientation, info.velocity);
        info.rotation = 0;
        return info;
    }
    static float NewOrientation(float current, Vector3 velocity){
        if(velocity.magnitude > 0){
            return Mathf.Atan2( -velocity.x, velocity.y) * Mathf.Rad2Deg;
        }
        return current;
    }
}

class KinematicArrivep{
    public static KinematicSteeringInfo getSteering(Vector3 target, Vector3 initialPos, ref float orientation, float maxSpeed, float radius){
        KinematicSteeringInfo result = new KinematicSteeringInfo();
        result.velocity = target - initialPos;
        if (result.velocity.magnitude < radius){
            return null;
        }
        result.velocity /= 0.25f;
        if(result.velocity.magnitude > maxSpeed){
            result.velocity.Normalize();
            result.velocity *= maxSpeed;
        }

        orientation = NewOrientation(orientation, result.velocity);
        result.rotation = 0;
        return result;
        
    }
    static float NewOrientation(float current, Vector3 velocity){
        if(velocity.magnitude > 0){
            return Mathf.Atan2( -velocity.x, velocity.y) * Mathf.Rad2Deg;
        }
        return current;
    }
}

class KinematicSteeringInfo{
    public Vector3 velocity;
    public float rotation;
}

public class Pathp{

    List<Vector3> Nodes;

    public Pathp(){
        Nodes = new List<Vector3>();
    }

    public void AddNode(Vector3 node) {
		Nodes.Add(node);
	}

    public List<Vector3> GetNodes(){
        return Nodes;
    }

}
