package jp.pinkikki.mqtt2sse

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class Mqtt2sseApplication

fun main(args: Array<String>) {
    runApplication<Mqtt2sseApplication>(*args)
}
