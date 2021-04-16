### Director for Open Match

> Manages the connection to Agones
> Assigns game servers to created matches.

### Requirements
> A Kubernetes cluster
> Open Match installed and running on the cluster

### How to Build
docker build . -t autochess-director

### How to Deploy
kubectl create namespace autochess
kubectl apply -f director.yaml

