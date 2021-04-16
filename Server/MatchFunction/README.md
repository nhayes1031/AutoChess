### Matchfunction logic for Open Match

> Generates proposals for matches from tickets created by the frontend API

### Requirements
> A Kubernetes cluster
> Open Match installed and running on the cluster

### How to Build
docker build . -t autochess-matchfunction

### How to Deploy
kubectl create namespace autochess
kubectl apply -f matchfunction.yaml