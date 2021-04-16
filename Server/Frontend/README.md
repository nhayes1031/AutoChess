### Frontend API for Game Client

> Utilizes Lidgren for UDP message processing
> Utilizes Open Match Frontend API for matchmaking (gRPC)

### Requirements
> A Kubernetes cluster
> Open Match installed and running on the cluster
> Port 34560 available

### How to Build
docker build . -t autochess-frontend

### How to Deploy
kubectl create namespace autochess
kubectl apply -f frontend.yaml

