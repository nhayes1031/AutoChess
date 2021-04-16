### Prerequisites
1. A Kubernetes cluster with the UDP port range 7000-8000 open on each node.
2. Agones controller installed in the target cluster.
3. kubectl properly configured.
4. WSL installed.
5. Have Helm installed.

### Setup Instructions
1. Create and start Agones on Minikube
  ```bash
   minikube start -p agones
   minikube profile agones
   minikube start --kubernetes-version v1.20.2 --vm-driver docker
   ```
2. Install Agones through Helm
   ```bash
   helm repo add agones https://agones.dev/chart/stable
   helm repo update
   helm install my-release --namespace agones-system --create-namespace agones/agones --set gameservers.minPort=7000,gameservers.maxPort=8000,helm.installTests=true
   ```
3. Run Helm test aginst the release
   ```bash
   helm test my-release --namespace agones-system
   ```
4. Verify Agones is running
   ```bash
   kubectl describe --namespace agones-system pods
   ```

   > The conditions for each pod should all be true.

### Useful Commands

## Kubectl
 > How to create a GameServer
 ```bash
 kubectl create -f <gameserver.yaml path>
 ```

 > How to add a Fleet
 ```bash
 kubectl apply -f <fleet.yaml path>
 ```

 > How to add a Fleet Autoscaler
 ```bash
 kubectl apply -f <fleetautoscaler.yaml path>
 ```

 > How to allocate a GameServer
 ```bash
 kubectl create -f <gameserverallocation.yaml path> -o yaml
 ```

 > How to show all GameServers
 ```bash
 kubectl get gameservers
 ```

 > How to show all Fleets
 ```bash
 kubectl get fleet
 ```

 > How to delete a GameServer
 ```bash
 kubectl delete gs <gameserver name>
 ```

 > How to show logs for a GameServer
 ```bash
 kubectl logs <pod name> <gameserver name>
 ```

 > List all pods for a namespace
 ```bash
 kubectl get pods --namespace agones-system
 ```

 > List all pods without a namespace
 ```bash
 kubectl get pods
 ```

 > Scale a fleet without recreating it
 ```bash
 kubectl scale fleet <gameserver name> --replicas=<number to scale to>
 ```

## Docker
 > How to create a new image
 ```bash
 docker build . -t <name of new image>
 ```

 > How to run a container
 ```bash
 docker run -dt <name of image>
 ```

 > How to get into a container
 ```bash
 docker exec -i <container id> bash
 ```

## Minikube
 > How to create and start Agones on Minikube
 ```bash
 minikube start -p agones
 minikube profile agones
 minikube start --kubernetes-version v1.20.2 --vm-driver docker
 ```

## Helm
 > List all namespaces
 ```bash
 helm list --all-namespaces
 ```

 > Uninstall the chart
 ```bash
 helm uninstall my-release --namespace=agones-system
 ```




### Installation Requirements
 1. Install Helm
 2. Install Docker
 3. Install Kubernetes
 4. Install Minikube
 5. Install Agones
 6. Install OpenMatch


### TODOS For this page.
 1. Create a domain diagram that maps out the relationship between OpenMatch and Agones
 2. Create a script for easy installation
 3. Create a script that starts up all of the local systems
 4. Create a domain diagram for the OpenMatch system
 5. Explain in your own words how OpenMatch works
 6. Explain in your own words how Agones works