
import requests

headers = {
        'Content-Type': 'application/json'
        }

requestResponse = requests.get("https://api.tiingo.com/tiingo/daily/spy/prices?token=6c3f73d6f280f1398e63db9e674fe2755bb79eef",
                                    headers=headers)

#for ()
#{

#}
print(requestResponse._content.adjClose)

