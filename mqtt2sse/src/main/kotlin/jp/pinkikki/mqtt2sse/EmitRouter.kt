package jp.pinkikki.mqtt2sse

import org.eclipse.paho.client.mqttv3.MqttClient
import org.eclipse.paho.client.mqttv3.MqttConnectOptions
import org.springframework.beans.factory.annotation.Value
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.web.reactive.function.server.ServerResponse
import org.springframework.web.reactive.function.server.body
import org.springframework.web.reactive.function.server.router
import org.springframework.web.reactive.function.server.sse
import reactor.core.publisher.Flux
import java.util.*

@Configuration
class EmitRouter {

    @Value("\${mqtt.server.endpoint}")
    val endpoint = ""

    @Bean
    fun route() = router {
        GET("/source") {
            var id = UUID.randomUUID().toString()
            var client = MqttClient("tcp://${endpoint}", id)

            var options = MqttConnectOptions()
            options.isAutomaticReconnect = true
            options.isCleanSession = true
            options.connectionTimeout = 10
            client.connect(options)

            ServerResponse.ok().sse().body(
                    Flux.create { sink ->
                        client.subscribe("/robot/topic") { _, message ->
                            var payload = message.payload
                            var data = String(payload)
                            sink.next(data)
                        }
                    }
            )
        }
    }
}
