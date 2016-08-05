input {
#	file {
#    	path => ["C:\Program Files\Apache Software Foundation\apache-tomcat-7.0.30\bin\logs\server.log"]  
#		start_position => "beginning"
#		add_field => { "source" => "Monitoring.RestFacade" }
#	}

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
			match => [ "message", "%{TIMESTAMP_ISO8601:timestamp} %{LOGLEVEL:level}\s+%{NOTSPACE:thread}\s+%{NOTSPACE:logger}\s+\[R:%{NOTSPACE:requestId}\s+S:%{NOTSPACE:sessionId}\s+%{NOTSPACE:userId}\s+%{WORD:userName}\] %{GREEDYDATA:body}" ]
		}

#		if [level] =~ /(?i)trace/ { 
#			drop {} 
#		} else if [level] =~ /(?i)debug/ { 
#			drop {}
#		} else {
			json {
				source => "body"
			}

			mutate {
				remove_field => [ "message", "body" ]		
			}
#		}
	}

	if [monitoring] == "Performance" {
		grok {
			patterns_dir => "./patterns"
			match => [ "message", "%{TIMESTAMP_ISO8601:timestamp} %{NOTSPACE:correlationId} %{WORD:component} %{NOTSPACE:userId} %{WORD:userName} %{NOTSPACE:action} %{NONNEGINT:duration} %{GREEDYDATA:result}" ]
		}

		if "_grokparsefailure" in [tags] { drop {} }

		mutate {
			convert => [ "duration", "integer" ]
			add_field => { "qualifiedAction" => "%{component}.%{action}" }
		}

		if [result] =~ "\<null\>" {
			mutate {
				remove_field => [ "result" ]
			}
		}
		
		if [correlationId] =~ "\<null\>" {
			mutate {
				remove_field => [ "correlationId" ]
			}
		}
		
		mutate {
			remove_field => [ "message" ]
		}
	}
}

output {
  elasticsearch { hosts => ["localhost:9200"] }
}