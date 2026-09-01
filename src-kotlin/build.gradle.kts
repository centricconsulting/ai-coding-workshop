import org.gradle.api.tasks.testing.Test
import org.jetbrains.kotlin.gradle.dsl.KotlinJvmProjectExtension

plugins {
    kotlin("jvm") version "2.0.21" apply false
}

allprojects {
    group = "com.example.taskmanager"
    version = "0.1.0"
}

subprojects {
    repositories {
        mavenCentral()
    }

    pluginManager.withPlugin("org.jetbrains.kotlin.jvm") {
        extensions.configure<KotlinJvmProjectExtension> {
            jvmToolchain(21)
        }

        dependencies {
            "testImplementation"("org.junit.jupiter:junit-jupiter:5.11.3")
            "testImplementation"("io.mockk:mockk:1.13.13")
            "testRuntimeOnly"("org.junit.platform:junit-platform-launcher")
        }

        tasks.withType<Test>().configureEach {
            useJUnitPlatform()
        }
    }
}
