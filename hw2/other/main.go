package main

import (
  "fmt"
  "math"
  // "math/rand"
  "sync"
  "runtime"
  "container/heap"
  "github.com/gonum/graph"
  "github.com/gonum/graph/path"
  "github.com/gonum/graph/simple"
  // "github.com/gonum/matrix/mat64"
)


func DijkstraFrom(u graph.Node, g graph.Graph) Shortest {
	if !g.Has(u) {
		return Shortest{from: u}
	}
	var weight path.Weighting
	if wg, ok := g.(graph.Weighter); ok {
		weight = wg.Weight
	} else {
		weight = path.UniformCost(g)
	}

	nodes := g.Nodes()
	path := newShortestFrom(u, nodes)

	// Dijkstra's algorithm here is implemented essentially as
	// described in Function B.2 in figure 6 of UTCS Technical
	// Report TR-07-54.
	//
	// This implementation deviates from the report as follows:
	// - the value of path.dist for the start vertex u is initialized to 0;
	// - outdated elements from the priority queue (i.e. with respect to the dist value)
	//   are skipped.
	//
	// http://www.cs.utexas.edu/ftp/techreports/tr07-54.pdf
	Q := priorityQueue{{node: u, dist: 0}}
	for Q.Len() != 0 {
		mid := heap.Pop(&Q).(distanceNode)
		k := path.indexOf[mid.node.ID()]
		if mid.dist > path.dist[k] {
			continue
		}
		for _, v := range g.From(mid.node) {
			j := path.indexOf[v.ID()]
			w, ok := weight(mid.node, v)
			if !ok {
				panic("dijkstra: unexpected invalid weight")
			}
			if w < 0 {
				panic("dijkstra: negative edge weight")
			}
			joint := path.dist[k] + w
			if joint < path.dist[j] {
				heap.Push(&Q, distanceNode{node: v, dist: joint})
				path.set(j, joint, k)
			}
		}
	}

	return path
}

type distanceNode struct {
	node graph.Node
	dist float64
}

// priorityQueue implements a no-dec priority queue.
type priorityQueue []distanceNode

func (q priorityQueue) Len() int            { return len(q) }
func (q priorityQueue) Less(i, j int) bool  { return q[i].dist < q[j].dist }
func (q priorityQueue) Swap(i, j int)       { q[i], q[j] = q[j], q[i] }
func (q *priorityQueue) Push(n interface{}) { *q = append(*q, n.(distanceNode)) }
func (q *priorityQueue) Pop() interface{} {
	t := *q
	var n interface{}
	n, *q = t[len(t)-1], t[:len(t)-1]
	return n
}




func BellmanFordSerial(s graph.Node, g graph.Graph) (p Shortest, ok bool) {
	// Your code goes here.
	// p := 
	if g == nil || !g.Has(s) {
		return Shortest{from: s}, true
	}
	var weight path.Weighting
	if wg, ok := g.(graph.Weighter); ok {
		weight = wg.Weight
	} else {
		weight = path.UniformCost(g)
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




//PG 31-33 in ch3-5.pdf

// predicated (pre()) for all edges(i,j) d[j] <= d[i] + w[i,j]

// code for thread j
// forall i != 0: d[i] = inf
//   fobidden(d, j) = There exists i in pre(j) : d[j] > d[i] + w[i, j]
// while exists j: forbidden(d, j)
//   forall j: forbidden(d, j)
//     d[j] = min((d[i] + w[i,j], i in pre(j)))

// func GetEdgesFrom(g graph.Graph, n graph.Node) []graph.Edge {
// 	var edges []graph.Edge
// 	for _, connected_nodes := g.From(n) {
// 		for _, n2 := range connected_nodes {
// 			edges := append(edges, g.Edge(n, n2))
// 		}
// 	}
// 	return edges
// }

func loop0(g graph.Graph, spath *Shortest, weight path.Weighting, nodes []graph.Node, wg *sync.WaitGroup, mtx *sync.Mutex) {
	var wg2 sync.WaitGroup
	for i := 1; i < len(nodes); i++ {
		for j, u := range nodes {
    		wg2.Add(1);
	    	go loop1(g, spath, weight, u, j, &wg2, mtx)
	    	wg2.Wait()
	    }
	}
	wg.Done()
}

func loop1(g graph.Graph, spath *Shortest, weight path.Weighting, u graph.Node, j int, wg2 *sync.WaitGroup, mtx *sync.Mutex) {
//  fmt.Println(n)
//  c <- i+10
//  fmt.Println(i+100)
//  close(c)
//	nodes := g.From(src)
//	forbidden := []graph.Node
	// changed := false
//	for j, u := range g.Nodes() {
		//go is_shorter_path(g, spath, src, dest)
	for _, v := range g.From(u) {
		k := spath.indexOf[v.ID()]
		w, _ := weight(u, v)
		// if !ok {
		// 	panic("bellman-ford: unexpected invalid weight")
		// }
		joint := spath.dist[j] + w
		if joint < spath.dist[k] {
			mtx.Lock()
			spath.set(k, joint, j)
			mtx.Unlock()
			// changed = true
		}
	}
//	}
	wg2.Done()
}

// func is_shorter_path(g graph.Graph, spath Shortest, src graph.Node, dest graph.Node) {
// 	u := spath.indexOf[src.ID()]
// 	v := spath.indexOf[dest.ID()]
// 	w, ok = weight(src, dest)
// 	if !ok {
// 		panic("bellman-ford: unexpected invalid weight")
// 	}
// 	/*
// 	joint := spath.dist[u] + w
//     if joint < spath.dist[v] {
//         spath.set(v, joint, u)
//         return true
//     }
//     return false
//     */
//     spath.set(v, math.Min(spath.dist[u] + w, spath.dist[v]), u)
// }

func BellmanFord(s graph.Node, g graph.Graph) (p Shortest, ok bool) {
	// Your code goes here.
	// var wg sync.WaitGroup
	NGoRoutines := runtime.GOMAXPROCS(runtime.NumCPU())
	var wg sync.WaitGroup
	var mtx sync.Mutex

	if(NGoRoutines == 10000) {
		fmt.Println("cool")
	}
	if !g.Has(s) {
		return Shortest{from: s}, true
	}
	var weight path.Weighting
	if wtr, ok := g.(graph.Weighter); ok {
		weight = wtr.Weight
	} else {
		weight = path.UniformCost(g)
	}

	nodes := g.Nodes()
	// n := len(nodes)
	spath := newShortestFrom(s, nodes)
	spath.dist[spath.indexOf[s.ID()]] = 0
	// fmt.Println(spath.dist)
    // fmt.Println(runtime.NumCPU())
    wg.Add(1)
    go loop0(g, &spath, weight, nodes, &wg, &mtx)
    wg.Wait()

    // for i := 1; i < len(nodes); i++ {
    // 	// c := make(chan int)
    // 	//go loop1(c chan int, g graph.Graph, spath Shortest, src graph.Node)
    // 	for j, u := range nodes {
    // 		wg.Add(1);
	   //  	go loop1(g, spath, weight, u, j, &wg, &mtx)
	   //  	wg.Wait()
	   //  }
    // }
	
	// for _, node := range nodes {

	// // for x := 0; x < n; x++ {
	// 	go loop1(c, g, spath, node)
	// 	// go init_dist(c, s, node)
	// 	// nid, val := <-c, 
	// 	// spath.dist[spath.indexOf[]] = <-c
	// 	// for i := range c {
	// 	// 	fmt.Println(i + 900)
	// 	// }
	// }

	// fmt.Println(spath.dist)
	


	return spath, true
}

func main() {
	for _, test := range ShortestPathTests {
	 	g := test.Graph()
		for _, e := range test.Edges {
			g.SetEdge(e)
		}
		fmt.Println(test.Name)

		// spath, kk := BellmanFordSerial(test.Query.From(), g.(graph.Graph))
		//fmt.Println(kk, spath)
		spath := DijkstraFrom(test.Query.From(), g.(graph.Graph))
		spath2, _ := BellmanFord(test.Query.From(), g.(graph.Graph))
		spath3, _ := BellmanFordSerial(test.Query.From(), g.(graph.Graph))

		p, weight := spath.To(test.Query.To())
		p2, weight2 := spath2.To(test.Query.To())
		p3, weight3 := spath3.To(test.Query.To())

		// fmt.Println(spath)
		fmt.Println(test.Weight, weight, p)
		// fmt.Println(spath2)
		fmt.Println(test.Weight, weight2, p2)
		// fmt.Println(spath3)
		fmt.Println(test.Weight, weight3, p3) 
		fmt.Println("\n")
	}
}










// Shortest is a shortest-path tree created by the BellmanFordFrom or DijkstraFrom
// single-source shortest path functions.
type Shortest struct {
    // from holds the source node given to
    // DijkstraFrom.
    from graph.Node

    // nodes hold the nodes of the analysed
    // graph.
    nodes []graph.Node
    // indexOf contains a mapping between
    // the id-dense representation of the
    // graph and the potentially id-sparse
    // nodes held in nodes.
    indexOf map[int]int

    // dist and next represent the shortest
    // paths between nodes.
    //
    // Indices into dist and next are
    // mapped through indexOf.
    //
    // dist contains the distances
    // from the from node for each
    // node in the graph.
    dist []float64
    // next contains the shortest-path
    // tree of the graph. The index is a
    // linear mapping of to-dense-id.
    next []int
}

func newShortestFrom(u graph.Node, nodes []graph.Node) Shortest {
    indexOf := make(map[int]int, len(nodes))
    uid := u.ID()
    for i, n := range nodes {
        indexOf[n.ID()] = i
        if n.ID() == uid {
            u = n
        }
    }

    p := Shortest{
        from: u,

        nodes:   nodes,
        indexOf: indexOf,

        dist: make([]float64, len(nodes)),
        next: make([]int, len(nodes)),
    }
    for i := range nodes {
        p.dist[i] = math.Inf(1)
        p.next[i] = -1
    }
    p.dist[indexOf[uid]] = 0

    return p
}

func (p Shortest) set(to int, weight float64, mid int) {
    p.dist[to] = weight
    p.next[to] = mid
}

// From returns the starting node of the paths held by the Shortest.
func (p Shortest) From() graph.Node { return p.from }

// WeightTo returns the weight of the minimum path to v.
func (p Shortest) WeightTo(v graph.Node) float64 {
    to, toOK := p.indexOf[v.ID()]
    if !toOK {
        return math.Inf(1)
    }
    return p.dist[to]
}

// To returns a shortest path to v and the weight of the path.
func (p Shortest) To(v graph.Node) (path []graph.Node, weight float64) {
    to, toOK := p.indexOf[v.ID()]
    if !toOK || math.IsInf(p.dist[to], 1) {
        return nil, math.Inf(1)
    }
    from := p.indexOf[p.from.ID()]
    path = []graph.Node{p.nodes[to]}
    for to != from {
        path = append(path, p.nodes[p.next[to]])
        to = p.next[to]
    }
    reverse(path)
    return path, p.dist[p.indexOf[v.ID()]]
}

// // AllShortest is a shortest-path tree created by the DijkstraAllPaths, FloydWarshall
// // or JohnsonAllPaths all-pairs shortest paths functions.
// type AllShortest struct {
//     // nodes hold the nodes of the analysed
//     // graph.
//     nodes []graph.Node
//     // indexOf contains a mapping between
//     // the id-dense representation of the
//     // graph and the potentially id-sparse
//     // nodes held in nodes.
//     indexOf map[int]int

//     // dist, next and forward represent
//     // the shortest paths between nodes.
//     //
//     // Indices into dist and next are
//     // mapped through indexOf.
//     //
//     // dist contains the pairwise
//     // distances between nodes.
//     dist *mat64.Dense
//     // next contains the shortest-path
//     // tree of the graph. The first index
//     // is a linear mapping of from-dense-id
//     // and to-dense-id, to-major with a
//     // stride equal to len(nodes); the
//     // slice indexed to is the list of
//     // intermediates leading from the 'from'
//     // node to the 'to' node represented
//     // by dense id.
//     // The interpretation of next is
//     // dependent on the state of forward.
//     next [][]int
//     // forward indicates the direction of
//     // path reconstruction. Forward
//     // reconstruction is used for Floyd-
//     // Warshall and reverse is used for
//     // Dijkstra.
//     forward bool
// }

// func newAllShortest(nodes []graph.Node, forward bool) AllShortest {
//     indexOf := make(map[int]int, len(nodes))
//     for i, n := range nodes {
//         indexOf[n.ID()] = i
//     }
//     dist := make([]float64, len(nodes)*len(nodes))
//     for i := range dist {
//         dist[i] = math.Inf(1)
//     }
//     return AllShortest{
//         nodes:   nodes,
//         indexOf: indexOf,

//         dist:    mat64.NewDense(len(nodes), len(nodes), dist),
//         next:    make([][]int, len(nodes)*len(nodes)),
//         forward: forward,
//     }
// }

// func (p AllShortest) at(from, to int) (mid []int) {
//     return p.next[from+to*len(p.nodes)]
// }

// func (p AllShortest) set(from, to int, weight float64, mid ...int) {
//     p.dist.Set(from, to, weight)
//     p.next[from+to*len(p.nodes)] = append(p.next[from+to*len(p.nodes)][:0], mid...)
// }

// func (p AllShortest) add(from, to int, mid ...int) {
// loop: // These are likely to be rare, so just loop over collisions.
//     for _, k := range mid {
//         for _, v := range p.next[from+to*len(p.nodes)] {
//             if k == v {
//                 continue loop
//             }
//         }
//         p.next[from+to*len(p.nodes)] = append(p.next[from+to*len(p.nodes)], k)
//     }
// }

// // Weight returns the weight of the minimum path between u and v.
// func (p AllShortest) Weight(u, v graph.Node) float64 {
//     from, fromOK := p.indexOf[u.ID()]
//     to, toOK := p.indexOf[v.ID()]
//     if !fromOK || !toOK {
//         return math.Inf(1)
//     }
//     return p.dist.At(from, to)
// }

// // Between returns a shortest path from u to v and the weight of the path. If more than
// // one shortest path exists between u and v, a randomly chosen path will be returned and
// // unique is returned false. If a cycle with zero weight exists in the path, it will not
// // be included, but unique will be returned false.
// func (p AllShortest) Between(u, v graph.Node) (path []graph.Node, weight float64, unique bool) {
//     from, fromOK := p.indexOf[u.ID()]
//     to, toOK := p.indexOf[v.ID()]
//     if !fromOK || !toOK || len(p.at(from, to)) == 0 {
//         if u.ID() == v.ID() {
//             return []graph.Node{p.nodes[from]}, 0, true
//         }
//         return nil, math.Inf(1), false
//     }

//     seen := make([]int, len(p.nodes))
//     for i := range seen {
//         seen[i] = -1
//     }
//     var n graph.Node
//     if p.forward {
//         n = p.nodes[from]
//         seen[from] = 0
//     } else {
//         n = p.nodes[to]
//         seen[to] = 0
//     }

//     path = []graph.Node{n}
//     weight = p.dist.At(from, to)
//     unique = true

//     var next int
//     for from != to {
//         c := p.at(from, to)
//         if len(c) != 1 {
//             unique = false
//             next = c[rand.Intn(len(c))]
//         } else {
//             next = c[0]
//         }
//         if seen[next] >= 0 {
//             path = path[:seen[next]]
//         }
//         seen[next] = len(path)
//         path = append(path, p.nodes[next])
//         if p.forward {
//             from = next
//         } else {
//             to = next
//         }
//     }
//     if !p.forward {
//         reverse(path)
//     }

//     return path, weight, unique
// }

// // AllBetween returns all shortest paths from u to v and the weight of the paths. Paths
// // containing zero-weight cycles are not returned.
// func (p AllShortest) AllBetween(u, v graph.Node) (paths [][]graph.Node, weight float64) {
//     from, fromOK := p.indexOf[u.ID()]
//     to, toOK := p.indexOf[v.ID()]
//     if !fromOK || !toOK || len(p.at(from, to)) == 0 {
//         if u.ID() == v.ID() {
//             return [][]graph.Node{{p.nodes[from]}}, 0
//         }
//         return nil, math.Inf(1)
//     }

//     var n graph.Node
//     if p.forward {
//         n = u
//     } else {
//         n = v
//     }
//     seen := make([]bool, len(p.nodes))
//     paths = p.allBetween(from, to, seen, []graph.Node{n}, nil)

//     return paths, p.dist.At(from, to)
// }

// func (p AllShortest) allBetween(from, to int, seen []bool, path []graph.Node, paths [][]graph.Node) [][]graph.Node {
//     if p.forward {
//         seen[from] = true
//     } else {
//         seen[to] = true
//     }
//     if from == to {
//         if path == nil {
//             return paths
//         }
//         if !p.forward {
//             reverse(path)
//         }
//         return append(paths, path)
//     }
//     first := true
//     for _, n := range p.at(from, to) {
//         if seen[n] {
//             continue
//         }
//         if first {
//             path = append([]graph.Node(nil), path...)
//             first = false
//         }
//         if p.forward {
//             from = n
//         } else {
//             to = n
//         }
//         paths = p.allBetween(from, to, append([]bool(nil), seen...), append(path, p.nodes[n]), paths)
//     }
//     return paths
// }

func reverse(p []graph.Node) {
    for i, j := 0, len(p)-1; i < j; i, j = i+1, j-1 {
        p[i], p[j] = p[j], p[i]
    }
}

















var ShortestPathTests = []struct {
	Name              string
	Graph             func() graph.EdgeSetter
	Edges             []simple.Edge
	HasNegativeWeight bool
	HasNegativeCycle  bool

	Query         simple.Edge
	Weight        float64
	WantPaths     [][]int
	HasUniquePath bool

	NoPathFor simple.Edge
}{
	// Positive weighted graphs.
	{
		Name:  "empty directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(1)},
		Weight: math.Inf(1),

		NoPathFor: simple.Edge{F: simple.Node(0), T: simple.Node(1)},
	},
	{
		Name:  "empty undirected",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(1)},
		Weight: math.Inf(1),

		NoPathFor: simple.Edge{F: simple.Node(0), T: simple.Node(1)},
	},
	{
		Name:  "one edge directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(1), W: 1},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(1)},
		Weight: 1,
		WantPaths: [][]int{
			{0, 1},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
	{
		Name:  "one edge self directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(1), W: 1},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(0)},
		Weight: 0,
		WantPaths: [][]int{
			{0},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
	{
		Name:  "one edge undirected",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(1), W: 1},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(1)},
		Weight: 1,
		WantPaths: [][]int{
			{0, 1},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
	{
		Name:  "two paths directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(2), W: 2},
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(2)},
		Weight: 2,
		WantPaths: [][]int{
			{0, 1, 2},
			{0, 2},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(1)},
	},
	{
		Name:  "two paths undirected",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(2), W: 2},
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(2)},
		Weight: 2,
		WantPaths: [][]int{
			{0, 1, 2},
			{0, 2},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(4)},
	},
	{
		Name:  "confounding paths directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->5 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(5), W: 1},

			// Add direct edge to goal of weight 4
			{F: simple.Node(0), T: simple.Node(5), W: 4},

			// Add edge to a node that's still optimal
			{F: simple.Node(0), T: simple.Node(2), W: 2},

			// Add edge to 3 that's overpriced
			{F: simple.Node(0), T: simple.Node(3), W: 4},

			// Add very cheap edge to 4 which is a dead end
			{F: simple.Node(0), T: simple.Node(4), W: 0.25},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(5)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 5},
			{0, 2, 3, 5},
			{0, 5},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "confounding paths undirected",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->5 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(5), W: 1},

			// Add direct edge to goal of weight 4
			{F: simple.Node(0), T: simple.Node(5), W: 4},

			// Add edge to a node that's still optimal
			{F: simple.Node(0), T: simple.Node(2), W: 2},

			// Add edge to 3 that's overpriced
			{F: simple.Node(0), T: simple.Node(3), W: 4},

			// Add very cheap edge to 4 which is a dead end
			{F: simple.Node(0), T: simple.Node(4), W: 0.25},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(5)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 5},
			{0, 2, 3, 5},
			{0, 5},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(5), T: simple.Node(6)},
	},
	{
		Name:  "confounding paths directed 2-step",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->5 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(5), W: 1},

			// Add two step path to goal of weight 4
			{F: simple.Node(0), T: simple.Node(6), W: 2},
			{F: simple.Node(6), T: simple.Node(5), W: 2},

			// Add edge to a node that's still optimal
			{F: simple.Node(0), T: simple.Node(2), W: 2},

			// Add edge to 3 that's overpriced
			{F: simple.Node(0), T: simple.Node(3), W: 4},

			// Add very cheap edge to 4 which is a dead end
			{F: simple.Node(0), T: simple.Node(4), W: 0.25},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(5)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 5},
			{0, 2, 3, 5},
			{0, 6, 5},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "confounding paths undirected 2-step",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->5 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(5), W: 1},

			// Add two step path to goal of weight 4
			{F: simple.Node(0), T: simple.Node(6), W: 2},
			{F: simple.Node(6), T: simple.Node(5), W: 2},

			// Add edge to a node that's still optimal
			{F: simple.Node(0), T: simple.Node(2), W: 2},

			// Add edge to 3 that's overpriced
			{F: simple.Node(0), T: simple.Node(3), W: 4},

			// Add very cheap edge to 4 which is a dead end
			{F: simple.Node(0), T: simple.Node(4), W: 0.25},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(5)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 5},
			{0, 2, 3, 5},
			{0, 6, 5},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(5), T: simple.Node(7)},
	},
	{
		Name:  "zero-weight cycle directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(1), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(1), W: 0},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight cycle^2 directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(1), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(1), W: 0},
			// With its own zero-weight cycle.
			{F: simple.Node(5), T: simple.Node(6), W: 0},
			{F: simple.Node(6), T: simple.Node(5), W: 0},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight cycle^2 confounding directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(1), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(1), W: 0},
			// With its own zero-weight cycle.
			{F: simple.Node(5), T: simple.Node(6), W: 0},
			{F: simple.Node(6), T: simple.Node(5), W: 0},
			// But leading to the target.
			{F: simple.Node(5), T: simple.Node(4), W: 3},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
			{0, 1, 5, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight cycle^3 directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(1), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(1), W: 0},
			// With its own zero-weight cycle.
			{F: simple.Node(5), T: simple.Node(6), W: 0},
			{F: simple.Node(6), T: simple.Node(5), W: 0},
			// With its own zero-weight cycle.
			{F: simple.Node(6), T: simple.Node(7), W: 0},
			{F: simple.Node(7), T: simple.Node(6), W: 0},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight 3·cycle^2 confounding directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(1), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(1), W: 0},
			// With 3 of its own zero-weight cycles.
			{F: simple.Node(5), T: simple.Node(6), W: 0},
			{F: simple.Node(6), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(7), W: 0},
			{F: simple.Node(7), T: simple.Node(5), W: 0},
			// Each leading to the target.
			{F: simple.Node(5), T: simple.Node(4), W: 3},
			{F: simple.Node(6), T: simple.Node(4), W: 3},
			{F: simple.Node(7), T: simple.Node(4), W: 3},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
			{0, 1, 5, 4},
			{0, 1, 5, 6, 4},
			{0, 1, 5, 7, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight reversed 3·cycle^2 confounding directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			// Add a path from 0->4 of weight 4
			{F: simple.Node(0), T: simple.Node(1), W: 1},
			{F: simple.Node(1), T: simple.Node(2), W: 1},
			{F: simple.Node(2), T: simple.Node(3), W: 1},
			{F: simple.Node(3), T: simple.Node(4), W: 1},

			// Add a zero-weight cycle.
			{F: simple.Node(3), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(3), W: 0},
			// With 3 of its own zero-weight cycles.
			{F: simple.Node(5), T: simple.Node(6), W: 0},
			{F: simple.Node(6), T: simple.Node(5), W: 0},
			{F: simple.Node(5), T: simple.Node(7), W: 0},
			{F: simple.Node(7), T: simple.Node(5), W: 0},
			// Each leading from the source.
			{F: simple.Node(0), T: simple.Node(5), W: 3},
			{F: simple.Node(0), T: simple.Node(6), W: 3},
			{F: simple.Node(0), T: simple.Node(7), W: 3},
		},

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
			{0, 5, 3, 4},
			{0, 6, 5, 3, 4},
			{0, 7, 5, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight |V|·cycle^(n/|V|) directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: func() []simple.Edge {
			e := []simple.Edge{
				// Add a path from 0->4 of weight 4
				{F: simple.Node(0), T: simple.Node(1), W: 1},
				{F: simple.Node(1), T: simple.Node(2), W: 1},
				{F: simple.Node(2), T: simple.Node(3), W: 1},
				{F: simple.Node(3), T: simple.Node(4), W: 1},
			}
			next := len(e) + 1

			// Add n zero-weight cycles.
			const n = 100
			for i := 0; i < n; i++ {
				e = append(e,
					simple.Edge{F: simple.Node(next + i), T: simple.Node(i), W: 0},
					simple.Edge{F: simple.Node(i), T: simple.Node(next + i), W: 0},
				)
			}
			return e
		}(),

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight n·cycle directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: func() []simple.Edge {
			e := []simple.Edge{
				// Add a path from 0->4 of weight 4
				{F: simple.Node(0), T: simple.Node(1), W: 1},
				{F: simple.Node(1), T: simple.Node(2), W: 1},
				{F: simple.Node(2), T: simple.Node(3), W: 1},
				{F: simple.Node(3), T: simple.Node(4), W: 1},
			}
			next := len(e) + 1

			// Add n zero-weight cycles.
			const n = 100
			for i := 0; i < n; i++ {
				e = append(e,
					simple.Edge{F: simple.Node(next + i), T: simple.Node(1), W: 0},
					simple.Edge{F: simple.Node(1), T: simple.Node(next + i), W: 0},
				)
			}
			return e
		}(),

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},
	{
		Name:  "zero-weight bi-directional tree with single exit directed",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: func() []simple.Edge {
			e := []simple.Edge{
				// Add a path from 0->4 of weight 4
				{F: simple.Node(0), T: simple.Node(1), W: 1},
				{F: simple.Node(1), T: simple.Node(2), W: 1},
				{F: simple.Node(2), T: simple.Node(3), W: 1},
				{F: simple.Node(3), T: simple.Node(4), W: 1},
			}

			// Make a bi-directional tree rooted at node 2 with
			// a single exit to node 4 and co-equal cost from
			// 2 to 4.
			const (
				depth     = 4
				branching = 4
			)

			next := len(e) + 1
			src := 2
			var i, last int
			for l := 0; l < depth; l++ {
				for i = 0; i < branching; i++ {
					last = next + i
					e = append(e, simple.Edge{F: simple.Node(src), T: simple.Node(last), W: 0})
					e = append(e, simple.Edge{F: simple.Node(last), T: simple.Node(src), W: 0})
				}
				src = next + 1
				next += branching
			}
			e = append(e, simple.Edge{F: simple.Node(last), T: simple.Node(4), W: 2})
			return e
		}(),

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(4)},
		Weight: 4,
		WantPaths: [][]int{
			{0, 1, 2, 3, 4},
			{0, 1, 2, 6, 10, 14, 20, 4},
		},
		HasUniquePath: false,

		NoPathFor: simple.Edge{F: simple.Node(4), T: simple.Node(5)},
	},

	// Negative weighted graphs.
	{
		Name:  "one edge directed negative",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(1), W: -1},
		},
		HasNegativeWeight: true,

		Query:  simple.Edge{F: simple.Node(0), T: simple.Node(1)},
		Weight: -1,
		WantPaths: [][]int{
			{0, 1},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
	{
		Name:  "one edge undirected negative",
		Graph: func() graph.EdgeSetter { return simple.NewUndirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node(0), T: simple.Node(1), W: -1},
		},
		HasNegativeWeight: true,
		HasNegativeCycle:  true,

		Query: simple.Edge{F: simple.Node(0), T: simple.Node(1)},
	},
	{
		Name:  "wp graph negative", // http://en.wikipedia.org/w/index.php?title=Johnson%27s_algorithm&oldid=564595231
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node('w'), T: simple.Node('z'), W: 2},
			{F: simple.Node('x'), T: simple.Node('w'), W: 6},
			{F: simple.Node('x'), T: simple.Node('y'), W: 3},
			{F: simple.Node('y'), T: simple.Node('w'), W: 4},
			{F: simple.Node('y'), T: simple.Node('z'), W: 5},
			{F: simple.Node('z'), T: simple.Node('x'), W: -7},
			{F: simple.Node('z'), T: simple.Node('y'), W: -3},
		},
		HasNegativeWeight: true,

		Query:  simple.Edge{F: simple.Node('z'), T: simple.Node('y')},
		Weight: -4,
		WantPaths: [][]int{
			{'z', 'x', 'y'},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
	{
		Name:  "roughgarden negative",
		Graph: func() graph.EdgeSetter { return simple.NewDirectedGraph(0, math.Inf(1)) },
		Edges: []simple.Edge{
			{F: simple.Node('a'), T: simple.Node('b'), W: -2},
			{F: simple.Node('b'), T: simple.Node('c'), W: -1},
			{F: simple.Node('c'), T: simple.Node('a'), W: 4},
			{F: simple.Node('c'), T: simple.Node('x'), W: 2},
			{F: simple.Node('c'), T: simple.Node('y'), W: -3},
			{F: simple.Node('z'), T: simple.Node('x'), W: 1},
			{F: simple.Node('z'), T: simple.Node('y'), W: -4},
		},
		HasNegativeWeight: true,

		Query:  simple.Edge{F: simple.Node('a'), T: simple.Node('y')},
		Weight: -6,
		WantPaths: [][]int{
			{'a', 'b', 'c', 'y'},
		},
		HasUniquePath: true,

		NoPathFor: simple.Edge{F: simple.Node(2), T: simple.Node(3)},
	},
}
