#include "guest.h"
#include "visit.h"
#include "constants.h"
#include "extensions.h"

#include <common\streamutil.h>
#include <common\strutil.h>
#include <common\parsers.h>

#include <map>
#include <algorithm>
#include <memory>
#include <ostream>
#include <exception>

namespace challenges {

    namespace park {

        std::vector<int> const & verify_distinct(
            std::vector<int> const & values) {

            std::map<int, int> check;

            for (auto const & x : values) {
                if (check.find(x) == check.end())
                    check.emplace(x, 0);
                else
                    check.emplace(x, check.at(x) + 1);
            }

            if (check.size() != values.size())
                throw std::exception();

            return values;
        }

        guest::guest(theme_park * p_theme_park, int entry_time_minutes, std::vector<int> const & desired)
            : _p_theme_park(p_theme_park)
            , _entry_time_minutes(entry_time_minutes)
            , _desired()
            , _sp_best_possible_visit() {

            set_desired(verify_distinct(desired));
        }

        guest::guest(guest const & other)
            : _p_theme_park(other._p_theme_park)
            , _entry_time_minutes(other._entry_time_minutes)
            , _desired(other._desired) {
        }

        guest::~guest() {
        }

        void guest::set_desired(std::vector<int> const & desired) {

            _desired.insert(_desired.end(), desired.cbegin(), desired.cend());

            /* Remember that if there are gaps with the hours then we have some choices
            * which ones to prefer, and which spaces to meet otherwise. */

            while (static_cast<int>(_desired.size()) < _p_theme_park->max_hours_per_day())
                _desired.push_back(constants::wait_time);
        }

        bool guest::try_visit() {

            auto & bp = _sp_best_possible_visit;

            if (bp != nullptr) return true;

            auto desired = std::vector<int>(_desired.begin(), _desired.end());

            std::sort(desired.begin(), desired.end());

            // Here's the clincher:
            while (std::next_permutation(desired.begin(), desired.end())) {

                auto candidate = visit(this, _p_theme_park,
                    std::vector<int>(desired.begin(), desired.end()));

                if (bp != nullptr && candidate.time_in_park() < bp->time_in_park())
                    bp.reset();

                if (bp == nullptr)
                    bp = std::make_unique<visit>(candidate);
            }

            return false;
        }

        void verify_within_park_hours(const int entry_time_minutes, const int max_hours_per_day)
        {
            /* For anything more involved than this, start looking into
            * a proper dimensional analysis, units of measure solution. */

            if (entry_time_minutes < max_hours_per_day*constants::minutes_per_hour)
                return;

            throw std::exception();
        }

        void guest::report(std::ostream & os) {

            auto & bp = _sp_best_possible_visit;

            if (bp == nullptr || bp->is_impossible())
                os << "IMPOSSIBLE" << std::endl;
            else
                os << bp->time_in_park() << std::endl;
        }

        std::vector<int> read_visits(std::string const & s, theme_park * p_theme_park) {

            split splitter;

            std::vector<int> result;

            for (auto const x : to_ints(splitter(s)))
                result.push_back(verify_range(x, 0, p_theme_park->attraction_count() - 1));

            return result;
        }

        guest guest::read_one(std::istream & is, theme_park * p_theme_park)
        {
            auto lines = read_lines(is, 3);

            // TODO: may check hours are not after park closed
            auto entry_time_minutes = to_int(lines[0]);

            verify_within_park_hours(entry_time_minutes, p_theme_park->max_hours_per_day());

            auto visit_count = verify_desired_attractions(to_int(lines[1]));

            auto visits = read_visits(lines[2], p_theme_park);

            verify_value(visits.size(), visit_count);

            return guest(p_theme_park, entry_time_minutes, visits);
        }

        std::vector<guest> guest::read_all(std::istream & is, int count, theme_park * p_theme_park)
        {
            std::vector<guest> result;

            while (count-- > 0)
                result.push_back(read_one(is, p_theme_park));

            return result;
        }
    }
}
