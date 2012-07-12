﻿module Edgy.DirectedGraph
#nowarn "62"

open Edgy.Core

/// Combines a sequence of paths, duplicate edges are ignored
let combine (paths: Path<_> seq) = 
    paths |> Seq.reduce (fun l r -> { r with Edges = Set.union l.Edges r.Edges })

/// Given a set of edges, find all leaves
let leaves edges = 
    edges |> Seq.collect (fun (l,r) -> [l;r]) |> Seq.countBy id |> Seq.filter (snd>>(=)1) |> Seq.map fst

/// Given a set of edges, find a set of all nodes
let allNodes edges = edges |> Seq.collect (fun (l,r) -> [l;r]) |> Set.ofSeq

/// Given a set of directed edges represented as tuples s.t. (a,b) implies a is the parent and b is the child, find all nodes and their parents 
let allNodesWithParents edges =
    edges |> Seq.collect (fun (l,r) -> [l, Set.empty; r, Set.singleton l]) 
    |> Seq.groupBy fst |> Seq.map (fun (n, seq) -> n, seq |> Seq.map snd |> Seq.reduce Set.union)

/// Given a set of directed edges represented as tuples s.t. (a,b) implies a is the parent and b is the child, find all nodes with parents and those parents
let nodesWithParents edges = edges |> Seq.groupBy snd |> Seq.map (fun (n, seq) -> n, seq |> Seq.map fst)

/// Given a set of edges, find all nodes and those they are connected to
let allNodesWithConnected edges =
    edges |> Seq.collect (fun (l,r) -> [l, Set.singleton r; r, Set.singleton l]) 
    |> Seq.groupBy fst |> Seq.map (fun (n, seq) -> n, seq |> Seq.map snd |> Seq.reduce Set.union)

/// Given a set of directed edges, find the minimum spanning tree with Prim's algorithm
//let primMST edges = 
//    let h :: t = edges
//    let V = Set.empty
//    let V_new = Set.singleton h
//    let E_new = Set.empty
//    while V_new <> V do
//        


// Example
let private totalGraph () = 
    [
        Path 2 <== 1
        Path 2 <== 1 ==> 3
        Path 2 ==> 4 <== 3 ==> 5
        Path 2 ==> 4 <== 3 <== 8
        Path 4 ==> 6
    ] |> combine

// val totalGraph : Path<int> =
//  {Current = 6;
//   Edges = set [(1, 2); (1, 3); (2, 4); (3, 4); (3, 5); (4, 6); (8, 3)];}
