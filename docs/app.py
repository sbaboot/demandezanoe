from waitress import serve
from flask import Flask, request, jsonify, make_response
from datetime import datetime
from modules import vinted
import gc

app = Flask(__name__)

vinted_url = "https://www.vinted.fr/vetements?"

@app.route('/api/vinted/infos', methods=['GET'])
def infos():
	with vinted.Vinted(vinted_url) as vtScraper:
		criteria = vtScraper.parseCriteria(request.get_json())
		start_time = datetime.now()
		req_infos = vtScraper.getNbPages(criteria)
		req_delay = datetime.now() - start_time
		print("Temps d'exécution de la requête : {}".format(req_delay.seconds))
	if (req_infos["error"] == 0):
		return make_response(jsonify(
			{"status": 1, "pages": req_infos["pages"], "results": req_infos['nbResults']}), 200)
	else:
		return make_response(jsonify({"status": 0, "message": req_infos['message']}), 200)


@app.route('/api/vinted', methods=['GET'])
def index():
	#gc.collect()

	with vinted.Vinted(vinted_url) as vtScraper:
		criteria = vtScraper.parseCriteria(request.get_json())

		start_time = datetime.now()

		results = vtScraper.get(criteria)

		req_delay = datetime.now() - start_time
		print("Temps d'exécution de la requête : {}".format(req_delay.seconds))


	if (results["error"] == 0):
		return make_response(
			jsonify({"status": 1, "items": results["items"], "delay": req_delay.seconds, "count": len(results["items"])}), 200)

	else:
		return make_response(jsonify(
			{"status": 0, "error": results["message"]}), 200)



if __name__ == "__main__":
	#app.run('0.0.0.0')
	serve(app, host='0.0.0.0', port=8000)
