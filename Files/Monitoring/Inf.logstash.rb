input {
	udp {
		port => 17651
		add_field => { "source" => "Performance.InfinniPlatform" }
		add_field => { "layer" => "InfinniPlatform" }
		add_field => { "monitoring" => "Performance" }
	}

	udp {
		port => 17652
		add_field => { "source" => "Events.InfinniPlatform" }
		add_field => { "layer" => "InfinniPlatform" }
		add_field => { "monitoring" => "Events" }
	}
}

filter {
	if [monitoring] == "Events" {
		grok {
			match => [ "message", "%{TIMESTAMP_ISO8601:timestamp}\|%{LOGLEVEL:level}\|%{NOTSPACE:thread}\|%{NOTSPACE:logger}\|%{NOTSPACE:requestId}\|%{NOTSPACE:sessionId}\|%{NOTSPACE:userId}\|%{NOTSPACE:userName}\|%{GREEDYDATA:body}" ]
		}

		json {
			source => "body"
		}

		mutate {
			remove_field => [ "message", "body" ]
		}
	}

	if [monitoring] == "Performance" {
		grok {
			match => [ "message", "%{TIMESTAMP_ISO8601:timestamp}\|%{NOTSPACE:correlationId}\|%{WORD:component}\|%{NOTSPACE:userId}\|%{NOTSPACE:userName}\|%{GREEDYDATA:body}" ]
		}

		if "_grokparsefailure" in [tags] { drop {} }

		json {
			source => "body"
		}

		if [body] =~ "\<null\>" {
			mutate {
				remove_field => [ "body" ]
			}
		}

		if [correlationId] =~ "\<null\>" {
			mutate {
				remove_field => [ "correlationId" ]
			}
		}

		mutate {
			remove_field => [ "message", "body" ]
		}
	}
}

output {
  elasticsearch { hosts => ["localhost:9200"] }
}