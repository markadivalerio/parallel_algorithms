package hw2
/*
import (
	// "fmt"
	"math"
	"sync"
	"github.com/gonum/graph"
)


// not implemented in final version.
func BellmanFordSerial(s graph.Node, g graph.Graph) (p Shortest, ok bool) {
	if g == nil || !g.Has(s) {
		return newShortestFrom(s, []graph.Node{s}), true
	}
	var weight Weighting
	if wg, ok := g.(graph.Weighter); ok {
		weight = wg.Weight
	} else {
		weight = UniformCost(g)
	}
	nodes := g.Nodes()

	spath := newShortestFrom(s, nodes)
	spath.dist[spath.indexOf[s.ID()]] = 0

	for i := 1; i < len(nodes); i++ {
		changed := false
		for j, u := range nodes {
			for _, v := range g.From(u) {
				k := spath.indexOf[v.ID()]
				w, ok := weight(u, v)
				if !ok {
					panic("bellman-ford: unexpected invalid weight")
				}
				joint := spath.dist[j] + w
				if joint < spath.dist[k] {
					spath.set(k, joint, j)
					changed = true
				}
			}
		}
		if !changed {
			break
		}
	}

	for j, u := range nodes {
		for _, v := range g.From(u) {
			k := spath.indexOf[v.ID()]
			w, ok := weight(u, v)
			if !ok {
				panic("bellman-ford: unexpected invalid weight")
			}
			if spath.dist[j]+w < spath.dist[k] {
				return spath, false
			}
		}
	}

	return spath, true
}



func outerloop(g graph.Graph, spath *Shortest, weight Weighting, nodes []graph.Node, wg *sync.WaitGroup, mtx *sync.Mutex) {
	var wg2 sync.WaitGroup
	for i := 1; i < len(nodes); i++ {
		for j, u := range nodes {
    		wg2.Add(1);
	    	go innerloop(g, spath, weight, u, j, &wg2, mtx)
	    	wg2.Wait()
	    }
	}
	wg.Done()
}

func innerloop(g graph.Graph, spath *Shortest, weight Weighting, u graph.Node, j int, wg2 *sync.WaitGroup, mtx *sync.Mutex) {
	for _, v := range g.From(u) {
		k := spath.indexOf[v.ID()]
		w, ok := weight(u, v)
		if !ok {
			panic("bellman-ford: unexpected invalid weight")
		}
		joint := spath.dist[j] + w
		if joint < spath.dist[k] {
			mtx.Lock()
			spath.set(k, joint, j)
			mtx.Unlock()
		}
	}
	wg2.Done()
}


func BellmanFordParallel(s graph.Node, g graph.Graph) (p Shortest, ok bool) {
	// Your code goes here.
	var wg sync.WaitGroup
	var mtx sync.Mutex

	if g == nil  || !g.Has(s) {
		return newShortestFrom(s, []graph.Node{s}), true
	}
	
	var weight Weighting
	if wtr, ok := g.(graph.Weighter); ok {
		weight = wtr.Weight
	} else {
		weight = UniformCost(g)
	}

	nodes := g.Nodes()
	spath := newShortestFrom(s, nodes)
    wg.Add(1)
    go outerloop(g, &spath, weight, nodes, &wg, &mtx)
    wg.Wait()

    // check for negative weight.

    // THIS IS NOT NORMAL BELLMAN-FORD BEHAVIOR, but rather this is for the
    // assignment, as hw2_test.go will fail this test if it contains ANY 
    // negative weights, while bellman-ford is known to be able to handle
    // negative weights. (Normally, it would perform one last check for negative
    // CYCLES, or more specifically, if doing 1 more iteration would continue to
    // change the final distance map)
    for _,val := range spath.dist {
		if val < 0 {
			panic("negative weight")
			// return spath, false
		}
	}

	// fmt.Println(spath.dist)

	return spath, true
}



















//This method will return minimum node
func minKeyInMap(numbers map[int]graph.Node) (minNumber int) {
	minNumber = math.MaxInt32
	for n := range numbers {
		if n < minNumber {
			minNumber = n
		}
	}
	return minNumber
}

//This method will return minimum index key in buckets
func minDeltaInBucket(numbers map[int]map[graph.Node]int) (minNumber int) {
	minNumber = math.MaxInt32
	for key, value := range numbers {
		if value != nil {
			if key < minNumber {
				minNumber = key
			}
		}

	}
	return minNumber
}

//This method relaxes the node. It will also reassign a node to apporpriate bucket .
func relax(s graph.Node, previousNode graph.Node, g graph.Graph, valueWeight float64, spath Shortest, wg *sync.WaitGroup, mtx *sync.Mutex) {
	defer wg.Done()
	key := spath.indexOf[s.ID()]
	dist := spath.dist[key]
	if valueWeight < dist {
		if dist != math.Inf(1) {
			index := int(dist) / spath.delta
			if _, found := spath.Buckets[index][s]; found {
				mtx.Lock()
				delete(spath.Buckets[index], s)
				mtx.Unlock()
			} else {
				mtx.Lock()
				if spath.Buckets[index] == nil {

					spath.Buckets[index] = make(map[graph.Node]int)

				}

				spath.Buckets[index][s] = s.ID()
				mtx.Unlock()
			}
		} else {
			index := int(valueWeight) / spath.delta
			mtx.Lock()
			if _, found := spath.Buckets[index][s]; !found {
				if spath.Buckets[index] == nil {

					spath.Buckets[index] = make(map[graph.Node]int)

				}

				spath.Buckets[index][s] = s.ID()
				mtx.Unlock()
			}

		}

		previousID := -1
		if previousNode != nil {
			previousID = spath.indexOf[previousNode.ID()]
		}
		mtx.Lock()
		spath.set(key, valueWeight, previousID)
		mtx.Unlock()
	}
}

//This method relaxes all nodes in a bucket
func relaxBucket(bucket map[graph.Node]float64, previousNodes map[graph.Node]graph.Node, g graph.Graph, spath Shortest, mtx *sync.Mutex) {
	var wg sync.WaitGroup

	for key, value := range bucket {
		previousNode := previousNodes[key]
		wg.Add(1)
		go relax(key, previousNode, g, value, spath, &wg, mtx)
	}
	wg.Wait()
}

//This will return all the nieboring edges with weight for all nodes in bucket
func getNeighboringEdges(bucket map[graph.Node]int, lightHeavyType string, g graph.Graph, spath Shortest) (map[graph.Node]float64, map[graph.Node]graph.Node) {
	var weight path.Weighting
	if wg, ok := g.(graph.Weighter); ok {
		weight = wg.Weight
	} else {
		weight = path.UniformCost(g)
	}
	neighborsWithWeight := make(map[graph.Node]float64)
	previousNodeMap := make(map[graph.Node]graph.Node)
	for u, _ := range bucket { // j
		for _, v := range g.From(u) {
			w, ok := weight(u, v)
			if !ok {
				panic("DeltaStep: unexpected invalid weight")
			}
			if w < 0 {
				panic("DeltaStep: negative edge weight")
			}
			edge_weight := spath.dist[spath.indexOf[u.ID()]] + w //j
			if lightHeavyType == "light" {
				if w <= float64(spath.delta) {
					if _, ok := neighborsWithWeight[v]; ok {
						if edge_weight < neighborsWithWeight[v] {
							neighborsWithWeight[v] = edge_weight
							previousNodeMap[v] = u
						}
					} else {
						neighborsWithWeight[v] = edge_weight
						previousNodeMap[v] = u
					}
				}
			} else if lightHeavyType == "heavy" {
				if w > float64(spath.delta) {
					if _, ok := neighborsWithWeight[v]; ok {
						if edge_weight <= neighborsWithWeight[v] {
							neighborsWithWeight[v] = edge_weight
							previousNodeMap[v] = u
						}
					} else {
						neighborsWithWeight[v] = edge_weight
						previousNodeMap[v] = u
					}
				}
			} else {
				panic("DeltaStep:Incorrect Edge")
			}

		}
		//  id	from := p.indexOf[]
		// for _,v :=range g.From()
	}
	return neighborsWithWeight, previousNodeMap
}

//This method will append to maps
func appendNodes(first map[graph.Node]int, second map[graph.Node]int) map[graph.Node]int {
	for key, value := range second {
		first[key] = value
	}
	return first
}

// Apply the delta-stepping algorihtm to Graph and return
// a shortest path tree.
//
// Note that this uses Shortest to make it easier for you,
// but you can use another struct if that makes more sense
// for the concurrency model you chose.
func DeltaStep(s graph.Node, g graph.Graph) Shortest {
	if !g.Has(s) {
		return Shortest{from: s} //return Shortest{}
	}
	nodes := g.Nodes()
	spath := newShortestFrom(s, nodes)
	spath.Buckets = make(map[int]map[graph.Node]int, spath.delta)
	for index, _ := range spath.Buckets {
		spath.Buckets[index] = make(map[graph.Node]int)
	}
	var mtx sync.Mutex
	var wgparent sync.WaitGroup
	wgparent.Add(1)
	go relax(s, nil, g, 0, spath, &wgparent, &mtx)
	wgparent.Wait()
	for len(spath.Buckets) > 0 {
		minDelta := minDeltaInBucket(spath.Buckets)

		r := make(map[graph.Node]int) //, len(g.Nodes()))
		for minDelta != math.MaxInt32 {
			req, previousNodes := getNeighboringEdges(spath.Buckets[minDelta], "light", g, spath)
			r = appendNodes(r, spath.Buckets[minDelta])
			delete(spath.Buckets, minDelta)
			relaxBucket(req, previousNodes, g, spath, &mtx)
			minDelta = minDeltaInBucket(spath.Buckets)
		}
		req, previousNodes := getNeighboringEdges(r, "heavy", g, spath) //req := getNeighboringEdges(spath.Buckets[minDelta], "heavy", g, spath)
		relaxBucket(req, previousNodes, g, spath, &mtx)
		// 		key := spath.Buckets[0]
		// for
	}

	fmt.Println(spath)
	return spath
	// Your code goes here.
	//return newShortestFrom(s, g.Nodes())
}



// Apply the bellman-ford algorihtm to Graph and return
// a shortest path tree.
//
// Note that this uses Shortest to make it easier for you,
// but you can use another struct if that makes more sense
// for the concurrency model you chose.
func BellmanFord(s graph.Node, g graph.Graph) Shortest {
	// Your code goes here.
	// pth, ok := BellmanFordSerial(s, g)
	pth, _ := BellmanFordParallel(s, g)
	
	return pth
}

// Apply the delta-stepping algorihtm to Graph and return
// a shortest path tree.
//
// Note that this uses Shortest to make it easier for you,
// but you can use another struct if that makes more sense
// for the concurrency model you chose.
// func DeltaStep(s graph.Node, g graph.Graph) Shortest {
// 	// Your code goes here.
// 	return newShortestFrom(s, g.Nodes())
// }

// Runs dijkstra from gonum to make sure that the tests are correct.
func Dijkstra(s graph.Node, g graph.Graph) Shortest {
	return DijkstraFrom(s, g)
}
*/