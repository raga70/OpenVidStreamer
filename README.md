# OpenVidStreamer
OpenVidStreamer is a video-sharing platform operating with its own unique economy, offering a monthly subscription model for users to access content. To incentivize content creation, the platform rewards users who upload videos by monetarily compensating them based on the watch time their uploads accumulate from other viewers. This system encourages active participation and content generation, enhancing the platform's value and diversity.
The platform features a recommendation algorithm that identifies videos a user likes and finds other users with similar tastes. It calculates a popularity score for each video based on likes, dislikes, and watch time. The algorithm also determines the user's top five preferred video categories. It combines these factors to rank videos, and the top-rated videos are then recommended to the user.

## Features: 

1. **Subscription Services**:
    - Users can activate or renew subscriptions through a payment portal.
    - The system handles subscription payments and provides confirmation of successful transactions.
    - An active subscription is required to consume content.
2. **Content Upload and Management**:
    - Any user can upload videos.
    - The platform supports entering metadata.
3. **Video Recommendations**:
    - Personalized video recommendations are generated based on users' watch history, liked videos, and preferred genres.
    - The recommendation algorithm considers user interactions to refine future suggestions.
4. **Video Playback**:
    - Interactions during playback, such as liking or disliking a video, are recorded and affect recommendations.
5. **Technical Architecture and Performance**:
    - The platform uses a microservices architecture, ensuring scalability and efficient management of services.
    - .NET, Kubernetes, and React are used.
    - API Gateway and microservice management tools like Ocelot and Consul are used for routing and service discovery.
6. **High Availability and Load Handling**:
    - Designed to handle significant loads, such as 10,000 video chunks per second for streaming.
    - Video uploads, logins, and other account activities are optimized for high concurrent user access.


<hr/>

Demo of the platform: [DemoVideoOfTheUI](https://youtu.be/RJKFKMjOKzw)

<hr/>

### Architecture:

#### DOCS:  [ProjectDocumentation](https://github.com/raga70/OpenVidStreamer/tree/main/Docs) 

C2:
![C2WithBackgroud](https://github.com/raga70/OpenVidStreamer/assets/8299535/fc696bc4-0a17-428e-914f-cf6eaa189642)





## Getting Started

### Installation

Download the latest OVF template from the releases section of this repository  to deploy on your preferred cloud provider or  deploy our Kubernetes files (mandatory for large-scale deployments), (frontend not built in the Kubernetes files, just preform a `vite build` and deploy it, ex. via nginx ).
OVF image:  [OpenVidStreamerExampleVMrelese](https://github.com/raga70/OpenVidStreamer/)
Kubernetes files: [OpenVidStreamerKubernetesFiles](https://github.com/raga70/OpenVidStreamer/tree/main/OpenVidStreamerKubernetesFiles)

Designed for Kubernetes environments, the microservices' container images are available at our [Docker Hub repository](https://hub.docker.com/u/openvidstreamer).



### Prerequisites

- Kubernetes engine (e.g., MicroK8s)
- Modify `account-deployment.yaml` as per the following   (location in OVF `~/OpenVidStreamerKubernetesFiles/account-deployment.yaml`):
    - `StripeSecretKey`: Obtain from [Stripe API keys](https://stripe.com/docs/keys)
    - `StripeRedirectUrl`: Update with your domain and port
    - `JwtSecret`: Generate a secure base64 string ([Generate Plus](https://generate.plus/en/base64))
    - `JwtExpiration`: Set the session duration in hours
![image](https://github.com/raga70/OpenVidStreamer/assets/8299535/07bd67e0-82f9-4312-ab7f-87f42262223d)


<hr/>


### port forwarding
Adjust port forwarding settings as necessary to ensure proper routing of service traffic.
![portForwards](https://github.com/raga70/OpenVidStreamer/assets/8299535/f604a491-0bc6-4e6f-97a7-c3713870b1ef)



<hr/>

### Databases (skip for OVF)

SQL files for empty databases: https://gist.github.com/raga70/175fe4ae885c2d644cd4f96616697659
#### need to scale up?
I will recommend using a managed database
simply do not use Kubernetes files for the databases, and modify the other ones to point to your managed database provider 

<hr/>


### Storage bucket (skip for OVF)
the Kubernetes deployment relies on an NFS server on the host machine, to store video files (do not spin it up in a pod you will have permissions problems!!!)
1. install an nfs server
2. modify the nfs Kubernetes files if necessary
#### need to scale up?
I will recommend to switch to a cloud storage bucket (AWS S3, Azure Blob) 

<hr/>



#### OVF built-in perks
the only thing you need to do to get it running is port forward, and input your data (steps 1 and  2)
the VM comes with the Observability stack so you can monitor your resource usage example https://youtu.be/9J7ks5oLtI8 , you can also monitor the service discovery through Consul


if you encounter any problems: SSH into your VM -> and delete all pods in the default namespace: `microk8s kubectl delete pods --all`
<br/>
<br/>
<br/>

notice: the VM has only 50GB assigned for video storage, so you might hit the limit pretty quickly, you can add an extra virtual hard drive and mount it to `/`
<br/>
<br/>
notice: the OVF template is just a starting example, it is of course recommended to host directly on AKS/GKS, use a managed database, and a storage bucket from your cloud platform. furthermore, if you are really getting a lot of traffic use your cloud provider\`s API gateway (you will need to re-engineer auth),   Ocelot (the project`s API gateway) is the slowest link in the system 

<hr/>

### Development
to run the microservices on bear metal you will need:
Consul Service Discovery, Docker container with Redis, RabbitMQ, Mysql <- create Databases and run `dotnet ef database update `(refer to C2)



