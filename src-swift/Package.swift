// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "TaskManager",
    platforms: [
        .iOS(.v17),
        .macOS(.v14),
    ],
    products: [
        .library(
            name: "TaskManagerDomain",
            targets: ["TaskManagerDomain"]
        ),
        .library(
            name: "TaskManagerApplication",
            targets: ["TaskManagerApplication"]
        ),
        .library(
            name: "TaskManagerInfrastructure",
            targets: ["TaskManagerInfrastructure"]
        ),
    ],
    targets: [
        .target(
            name: "TaskManagerDomain"
        ),
        .target(
            name: "TaskManagerApplication",
            dependencies: ["TaskManagerDomain"]
        ),
        .target(
            name: "TaskManagerInfrastructure",
            dependencies: ["TaskManagerDomain"]
        ),
        .testTarget(
            name: "TaskManagerDomainTests",
            dependencies: ["TaskManagerDomain"]
        ),
        .testTarget(
            name: "TaskManagerApplicationTests",
            dependencies: [
                "TaskManagerApplication",
                "TaskManagerDomain",
                "TaskManagerInfrastructure",
            ]
        ),
        .testTarget(
            name: "TaskManagerInfrastructureTests",
            dependencies: [
                "TaskManagerDomain",
                "TaskManagerInfrastructure",
            ]
        ),
    ]
)
