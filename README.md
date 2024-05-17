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

Demo of the platform:

<hr/>

Architecture:

DOCS: 

![C2](https://github.com/raga70/OpenVidStreamer/assets/8299535/8b049baf-dba5-4f65-bb5e-21c5fda00201)


OpenVidStreamer is a microservice architecture designed for horizontal scaling


# How To Install

In the releases of this repository, you will find  OVF template, you can install it in your favorite cloud provider 


this system is built to be deployed on  Kubernetes, but if you want to do your own thing, the microservices` container images can be found at https://hub.docker.com/u/openvidstreamer

#### prerequisite:
microk8s or any other Kubernetes engine ,


#### 1 modify the account-deployment.yaml variables 

location of the file in release .vmdk/ovf  `~/OpenVidStreamerKubernetesFiles/account-deployment.yaml`

1.1 `StripeSecretKey` -  For processing payments -  go and make a free account on https://stripe.com/ ,  then navigate to Developers -> API keys -> Copy the SecretKey 
1.2 `StripeRedirectUrl` - change this env var to with your public domain and port 
1.3 `JwtSecret` - REPLACE THIS !!!  you can use https://generate.plus/en/base64 to geneare a secure secret use at least 30chars
1.4 `JwtExpiration` - this is the time in hours that a user will stay logged in inside the platform without the need of re-auth 



![image](https://github.com/raga70/OpenVidStreamer/assets/8299535/07bd67e0-82f9-4312-ab7f-87f42262223d)





#### port forwarding

![portForwards](https://github.com/raga70/OpenVidStreamer/assets/8299535/f604a491-0bc6-4e6f-97a7-c3713870b1ef)





#### Databases (skip for OVF)

SQL files for empty databases: https://gist.github.com/raga70/175fe4ae885c2d644cd4f96616697659

simply do not use Kubernetes files for the databases, and modify the other once to point to your managed database provider 



#### Storage bucket (skip for OVF)
the Kubernetes deployment relies on an NFS server on the host machine, to store video files (do not spin it up in a pod it will not work!!!)
1. install an nfs server
2. modify the nfs Kubernetes files if necessary
###### need to scale up?
I will recommend to switch to a cloud storage bucket (AWS S3, Azure Blob) 




#### OVF built-in perks
the only thing you need to do to get it running is port forward, and input your data (steps 1 and  2)
the VM comes with the Observability stack so you can monitor your resource usage example https://youtu.be/9J7ks5oLtI8 , you can also monitor the service discovery through Consul


if you encounter any problems: SSH into your vm -> and delete all pods in the default namespace: `microk8s kubectl delete pods --all`

notice: the VM has only 50GB assigned for video storage, so you might hit the limit pretty quickly, you can add an extra virtual hard drive and mount it to `/`
notice: the OVF is just a starting example, it is of course recommended to host directly on AKS/GKS, use a managed database, and a storage bucket from your cloud platform. furthermore if you are really getting a lot of traffic use your cloud provider`s API gateway (you will need to re-engineer auth)


### Development
to run the microservices on bear metal you will need:
`Consul Service Discovary`, `Docker container with Redis`, `RabbitMQ`, `Mysql` <- create Databases and run `dotnet ef database update (refer to C2)`



